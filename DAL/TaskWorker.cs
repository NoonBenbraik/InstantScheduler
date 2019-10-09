using InstantScheduler.Meta;
using InstantScheduler.Models;
using InstaSharper.API;
using InstaSharper.Classes;
using InstaSharper.Classes.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace InstantScheduler.DAL
{
    public static class TaskWorker
    {
        public static void StartBackgroundWorker(int UserId, IInstaApi Api)
        {
            Console.WriteLine("StartBackgroundWorker");

            UserModel User;
            using (var context = new InstaContext())
            {
                User = context.Users.Include("Schedules").Include("Tasks").Include("Searches").FirstOrDefault(u => u.Id == UserId);
            }

            List<TaskModel> tasks = new List<TaskModel>();

            System.Timers.Timer timer_1 = new System.Timers.Timer();
            timer_1.Elapsed += (object sender, ElapsedEventArgs e) =>
            {
                Console.WriteLine("Timer_1 elapsed...");
                User.Schedules.Where(sc => sc.Active).ToList().ForEach(sc =>
                {
                    tasks.AddRange(sc.Tasks.Where(t => t.Active && !tasks.Contains(t)));
                });
            };
            timer_1.Interval = 5000;
            timer_1.Enabled = true;

            System.Timers.Timer timer_2 = new System.Timers.Timer();
            timer_2.Elapsed += (object sender, ElapsedEventArgs e) =>
            {
                Console.WriteLine("Timer_2 elapsed...");

                tasks.AsParallel().ForAll(t =>
                {
                    if (t.Active)
                        RunTask(t, Api);
                });
            };

            timer_2.Interval = 2000;
            timer_2.Enabled = true;
        }

        private static void RunTask(TaskModel t, IInstaApi Api)
        {
            switch (t.TaskType)
            {
                case TaskType.Comment:
                    RunCommentTask(t, Api);
                    break;
                case TaskType.DM:
                    RunDMTask(t, Api);
                    break;
                case TaskType.Follow:
                    RunFollowTask(t, Api);
                    break;
                case TaskType.Like:
                    RunLikeTask(t, Api);
                    break;
                case TaskType.Post:
                    RunPostTaskAsync(t, Api);
                    break;
                case TaskType.Unfollow:
                    RunUnfollowTaskAsync(t, Api);
                    break;
                case TaskType.Unlike:
                    RunUnlikeTaskAsync(t, Api);
                    break;
            }

            using (var context = new InstaContext())
            {
                context.Tasks.First(ts => ts.Id == t.Id).Exectued++;
                context.SaveChanges();
            }

            Helper.LogTaskExecute(t);
        }


        // Done
        private static async Task RunUnlikeTaskAsync(TaskModel t, IInstaApi api)
        {
            Console.WriteLine("RunUnlikeTask");

            List<InstaMedia> medias = new List<InstaMedia>();

            var _temp = await api.GetLikeFeedAsync(PaginationParameters.MaxPagesToLoad(5));

            if (_temp.Succeeded)
            {
                medias.AddRange(_temp.Value); 
            }

            // execute
            medias.Sort((m1, m2) => m2.LikesCount - m1.LikesCount);

            medias = medias.Take(1).ToList();

            medias.ForEach(async m =>
            {
                var result = await api.UnLikeMediaAsync(m.InstaIdentifier);
                if (result.Succeeded)
                {
                    Console.WriteLine("Media Unliked: " + result.Value);
                }
            });
        }


        // Done
        private static async Task RunUnfollowTaskAsync(TaskModel t, IInstaApi api)
        {
            Console.WriteLine("RunUnfollowTask");
            List<InstaUserShort> users = new List<InstaUserShort>();
            var _currentUser = await api.GetCurrentUserAsync();
            InstaCurrentUser currentUser; 

            if (_currentUser.Succeeded)
            {
                currentUser = _currentUser.Value; 

                t.Searches.ForEach(s =>
                {
                    JsonConvert.DeserializeObject<List<string>>(s.InStrings).ForEach(async inString =>
                    {
                        var temp = await api.GetUserFollowingAsync(currentUser.UserName, PaginationParameters.MaxPagesToLoad(5), inString);
                        if (temp.Succeeded)
                        {
                            users.AddRange(temp.Value);
                        }
                    });


                    JsonConvert.DeserializeObject<List<string>>(s.InStrings).ForEach(async exString =>
                    {
                        var temp = await api.GetUserFollowingAsync(currentUser.UserName, PaginationParameters.MaxPagesToLoad(5), exString);
                        if (temp.Succeeded)
                        {
                            users.RemoveAll(u => temp.Value.Contains(u));
                        }
                    });


                    users = users.Take(1).ToList(); 

                    users.ForEach(async u =>
                    {
                        var result = await api.UnFollowUserAsync(u.Pk);
                        Console.WriteLine("User followed: " + u.FullName);
                    });
                });
            }

        }

        // DONE
        private static async Task RunPostTaskAsync(TaskModel t, IInstaApi api)
        {
            
            if (t.GetValues().Images.Count > 0)
            {
                if(t.GetValues().Images.Count > 1)
                {
                    var _imgs = new List<InstaImage>();
                    t.GetValues().Images.ForEach(img =>
                    {
                        _imgs.Add(new InstaImage
                        {
                            URI = img.StandardResolution.Url,
                            Height = img.StandardResolution.Height,
                            Width = img.StandardResolution.Width
                        });
                    }); 

                    var _temp = await api.UploadPhotosAlbumAsync(_imgs.ToArray(), t.GetValues().Text);

                    if (_temp.Succeeded)
                    {
                        // Success
                    }
                }
                else
                {
                    var img = t.GetValues().Images.First(); 

                    var _img = new InstaImage
                    {
                        URI = img.StandardResolution.Url,
                        Height = img.StandardResolution.Height,
                        Width = img.StandardResolution.Width
                    };

                    var _temp = await api.UploadPhotoAsync(_img, t.GetValues().Text); 
                    if (_temp.Succeeded)
                    {
                        // Success
                    }
                }
            }
            else if (t.GetValues().Videos.Count > 0)
            {
                var vid = t.GetValues().Videos.First();

                InstaVideo _vid = new InstaVideo
                (
                    vid.StandardResolution.Url,
                    vid.StandardResolution.Width,
                    vid.StandardResolution.Height,
                    1
                );
                var _img = new InstaImage
                {
                    URI = @"\src\img\Header.jpg",
                    Height = 100,
                    Width = 100
                };

                var _temp = await api.UploadVideoAsync(_vid, _img, t.GetValues().Text);

                if (_temp.Succeeded)
                {

                }
            }
        }

        // Done
        private static void RunLikeTask(TaskModel t, IInstaApi api)
        {
            Console.WriteLine("RunLikeTask");

            List<InstaMedia> medias = new List<InstaMedia>();

            t.Searches.ForEach(s =>
            {
                // Search by Users
                if (s.InUsers)
                {
                    var users = new List<InstaUserShort>();
                    JsonConvert.DeserializeObject<List<string>>(s.InStrings).ForEach(async inString =>
                    {
                        var temp = await api.SearchUsersAsync(inString);
                        if (temp.Succeeded)
                        {
                            users.AddRange(temp.Value.ToList());
                        }
                    });

                    JsonConvert.DeserializeObject<List<string>>(s.ExStrings).ForEach(async exString =>
                    {
                        var temp = await api.SearchUsersAsync(exString);
                        if (temp.Succeeded)
                        {
                            users.RemoveAll(u => temp.Value.Contains(u));
                        }
                    });


                    users.ForEach(async u =>
                    {
                        var temp = await api.GetUserMediaAsync(u.UserName, PaginationParameters.MaxPagesToLoad(1));
                        if (temp.Succeeded)
                        {
                            medias.AddRange(temp.Value.Where(m => !m.HasLiked).Take(5));
                        }
                    });
                }

            });
            // End of Search by Users



            // execute
            medias = medias.Where(m => !m.HasLiked).ToList();
            medias.Sort((m1, m2) => m1.LikesCount - m2.LikesCount);

            medias = medias.Take(1).ToList();

            medias.ForEach(async m =>
            {
                var result = await api.LikeMediaAsync(m.InstaIdentifier);
                if (result.Succeeded)
                {
                    Console.WriteLine("Media Liked: " + result.Value);
                }
            });
        }

        // Done
        private static void RunFollowTask(TaskModel t, IInstaApi api)
        {
            Console.WriteLine("RunFollowTask");

            List<InstaUserShort> users = new List<InstaUserShort>();

            t.Searches.ForEach(s =>
            {
                JsonConvert.DeserializeObject<List<string>>(s.InStrings).ForEach(async inString =>
                {
                    var temp = await api.SearchUsersAsync(inString);
                    if (temp.Succeeded)
                    {
                        users.AddRange(temp.Value);
                    }
                });


                JsonConvert.DeserializeObject<List<string>>(s.ExStrings).ForEach(async exString =>
                {
                    var temp = await api.SearchUsersAsync(exString);
                    if (temp.Succeeded)
                    {
                        users.RemoveAll(u => temp.Value.Contains(u));
                    }
                });


                var _users = new List<InstaUserShort>();

                users.ForEach(async u =>
                {
                    var isFollowed = await api.GetFriendshipStatusAsync(u.Pk);

                    if (isFollowed.Succeeded)
                    {
                        if (!isFollowed.Value.Following)
                        {
                            _users.Add(u);
                        }
                    }
                });

                _users = _users.Take(1).ToList();

                _users.ForEach(async u =>
                {
                    var result = await api.FollowUserAsync(u.Pk);
                    Console.WriteLine("User followed: " + u.FullName);
                });
            }); 
        }

        // DONE
        private static void RunDMTask(TaskModel t, IInstaApi api)
        {
            Console.WriteLine("RunDMTask");

            t.Searches.ForEach(s =>
            {
                // Search by Users
                var users = new List<InstaUserShort>();
                JsonConvert.DeserializeObject<List<string>>(s.InStrings).ForEach(async inString =>
                {
                    var temp = await api.SearchUsersAsync(inString);
                    if (temp.Succeeded)
                    {
                        users.AddRange(temp.Value.ToList());
                    }
                });

                JsonConvert.DeserializeObject<List<string>>(s.ExStrings).ForEach(async exString =>
                {
                    var temp = await api.SearchUsersAsync(exString);
                    if (temp.Succeeded)
                    {
                        users.RemoveAll(u => temp.Value.Contains(u));
                    }
                });

                users = users.Take(1).ToList();

                users.ForEach(async u =>
                {
                    var _temp = await api.SendDirectMessage(u.UserName, "", t.GetValues().Text);

                    if (_temp.Succeeded)
                    {

                    }
                });
                

            });
            // End of Search by Users

        }

        // DONE
        private static void RunCommentTask(TaskModel t, IInstaApi api)
        {
            Console.WriteLine("RunCommentTask");


            List<InstaMedia> medias = new List<InstaMedia>();

            t.Searches.ForEach(s =>
            {
                // Search by Users
                if (s.InUsers)
                {
                    var users = new List<InstaUserShort>();
                    JsonConvert.DeserializeObject<List<string>>(s.InStrings).ForEach(async inString =>
                    {
                        var temp = await api.SearchUsersAsync(inString);
                        if (temp.Succeeded)
                        {
                            users.AddRange(temp.Value.ToList());
                        }
                    });

                    JsonConvert.DeserializeObject<List<string>>(s.ExStrings).ForEach(async exString =>
                    {
                        var temp = await api.SearchUsersAsync(exString);
                        if (temp.Succeeded)
                        {
                            users.RemoveAll(u => temp.Value.Contains(u));
                        }
                    });


                    users.ForEach(async u =>
                    {
                        var temp = await api.GetUserMediaAsync(u.UserName, PaginationParameters.MaxPagesToLoad(1));
                        if (temp.Succeeded)
                        {
                            medias.AddRange(temp.Value.Where(m => !m.HasLiked).Take(5));
                        }
                    });
                }

            });
            // End of Search by Users



            // execute
            medias.Sort((m1, m2) => m1.PreviewComments.Count - m2.PreviewComments.Count);

            medias = medias.Take(1).ToList();

            medias.ForEach(async m =>
            {
                var result = await api.CommentMediaAsync(m.InstaIdentifier, t.GetValues().Text);
                if (result.Succeeded)
                {
                    Console.WriteLine("Media Commented on: " + result.Value);
                }
            });
        }
    }
}

