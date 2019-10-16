using InstantScheduler.Meta;
using InstantScheduler.Models;
using InstaSharper.API;
using InstaSharper.Classes;
using InstaSharper.Classes.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Windows;

namespace InstantScheduler.DAL
{
    public static class TaskWorker
    {
        public static void StartBackgroundWorker(int UserId, IInstaApi Api)
        {
            MessageBox.Show("StartBackgroundWorker", "MileStone", MessageBoxButton.OK, MessageBoxImage.Information);

            UserModel User;
            using (var context = new InstaContext())
            {
                User = context.Users.Include("Schedules").Include("Tasks").Include("Searches").FirstOrDefault(u => u.Id == UserId);
            }

            List<TaskModel> tasks = new List<TaskModel>();

            System.Timers.Timer timer_1 = new System.Timers.Timer();
            timer_1.Elapsed += (object sender, ElapsedEventArgs e) =>
            {
                User.Schedules.Where(sc => sc.Active).ToList().ForEach(sc =>
                {
                    tasks.AddRange(sc.Tasks.Where(t => t.Active && !tasks.Contains(t)));
                    tasks.RemoveAll(t => !t.Active); 
                });
            };
            timer_1.Interval = 20000;
            timer_1.Enabled = true;

            System.Timers.Timer timer_2 = new System.Timers.Timer();
            timer_2.Elapsed += (object sender, ElapsedEventArgs e) =>
            {
                tasks.ForEach(t =>
                {
                    if (t.Active)
                        RunTask(t, Api);
                });
            };

            timer_2.Interval = 10000;
            timer_2.Enabled = true;
        }

        private static void RunTask(TaskModel t, IInstaApi Api)
        {
            MessageBox.Show($"Task Type: {t.TaskType.ToString()} {Environment.NewLine}Task Name: {t.Name}", "Task Running", MessageBoxButton.OK, MessageBoxImage.Information);

            switch (t.TaskType)
            {
                case TaskType.Comment:
                    RunCommentTaskAsync(t, Api);
                    break;
                case TaskType.DM:
                    RunDMTaskAsync(t, Api);
                    break;
                case TaskType.Follow:
                    RunFollowTaskAsync(t, Api);
                    break;
                case TaskType.Like:
                    RunLikeTaskAsync(t, Api);
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

        }


        private static async void RunUnlikeTaskAsync(TaskModel t, IInstaApi api)
        {
            t = t.Refreshed;

            List<InstaMedia> medias = new List<InstaMedia>();

            var _temp = await api.GetLikeFeedAsync(PaginationParameters.MaxPagesToLoad(5));

            if (_temp.Succeeded)
            {
                medias.AddRange(_temp.Value);

                var FilteredMedia = await Helper.GetFilteredMediaAsync(medias, t.Searches, api, 5); 
                
                if (FilteredMedia.Count > 0)
                {
                    foreach(var media in FilteredMedia)
                    {
                        if (media != null)
                        {
                            var result = await api.UnLikeMediaAsync(media.InstaIdentifier);

                            if (result.Succeeded)
                            {
                                t.TaskExecuted();
                                Helper.Log($"Task Executed: {t.Name} - {t.TaskType.ToString()} {Environment.NewLine} {JsonConvert.SerializeObject(result.Value)}");
                            }
                            else
                            {
                                Helper.Log(JsonConvert.SerializeObject(result.Info));
                            }
                        }
                    }
                    
                }


            }

            

            
        }

        private static async void RunLikeTaskAsync(TaskModel t, IInstaApi api)
        {
            List<InstaMedia> medias = new List<InstaMedia>();

            t = t.Refreshed;

            var _temp = await api.GetUserTimelineFeedAsync(PaginationParameters.MaxPagesToLoad(5));

            if (_temp.Succeeded)
            {
                medias.AddRange(_temp.Value.Medias);

                var FilteredMedia = await Helper.GetFilteredMediaAsync(medias, t.Searches, api, 5); 


                // Execute
                if (FilteredMedia.Count > 0)
                {
                    foreach(var media in FilteredMedia)
                    {
                        if (media != null)
                        {
                            var result = await api.LikeMediaAsync(media.InstaIdentifier);

                            if (result.Succeeded)
                            {
                                t.TaskExecuted();
                                Helper.Log($"Task Executed: {t.Name} - {t.TaskType.ToString()} {Environment.NewLine} {JsonConvert.SerializeObject(result.Value)}");
                            }
                            else
                            {
                                Helper.Log(JsonConvert.SerializeObject(result.Info)); 
                            }
                        }
                    }
                    
                }
            }
           
        }

        private static async void RunFollowTaskAsync(TaskModel t, IInstaApi api)
        {
            List<InstaUserShort> users = new List<InstaUserShort>();

            t = t.Refreshed;

            foreach(var search in t.Searches)
            {

                foreach(var inString in search.GetInStrings())
                {
                    var _temp = await api.SearchUsersAsync(inString);

                    if (_temp.Succeeded)
                    {
                        users.AddRange(_temp.Value.ToList()); 
                    }
                }

                var FilteredUsers = await Helper.GetFilteredUsersAsync(users, t.Searches, api, 5);

                // Execute
                if (FilteredUsers.Count > 0)
                {
                    foreach (var user in FilteredUsers)
                    {
                        if (user != null)
                        {
                            var result = await api.FollowUserAsync(user.Pk);

                            if (result.Succeeded)
                            {
                                if (result.Value.Following)
                                {
                                    t.TaskExecuted();
                                    Helper.Log($"Task Executed: {t.Name} - {t.TaskType.ToString()} {Environment.NewLine} {JsonConvert.SerializeObject(result.Value)}");
                                }
                                else
                                    Helper.Log(JsonConvert.SerializeObject(result.Value)); 
                            }
                        }
                    }

                }
            }
        }

        private static async void RunUnfollowTaskAsync(TaskModel t, IInstaApi api)
        {
            t = t.Refreshed;

            List<InstaUserShort> users = new List<InstaUserShort>();
            var _currentUser = await api.GetCurrentUserAsync();

            if (_currentUser.Succeeded)
            {
                var _followers = await api.GetUserFollowersAsync(_currentUser.Value.UserName, PaginationParameters.MaxPagesToLoad(10)); 

                if (_followers.Succeeded)
                {
                    var filteredUsers = await Helper.GetFilteredUsersAsync(_followers.Value.ToList(), t.Searches, api, 5); 

                    foreach(var user in filteredUsers)
                    {
                        if(user != null)
                        {
                            var result = await api.UnFollowUserAsync(user.Pk);
                            if (result.Succeeded)
                                if (!result.Value.Following)
                                {
                                    t.TaskExecuted();
                                    Helper.Log($"Task Executed: {t.Name} - {t.TaskType.ToString()} {Environment.NewLine} {JsonConvert.SerializeObject(result.Value)}");
                                }
                                else
                                    Helper.Log(JsonConvert.SerializeObject(result.Value));
                        }
                    }
                }   
            }

        }

        private static async void RunPostTaskAsync(TaskModel t, IInstaApi api)
        {
            t = t.Refreshed;

            if (t.GetValues().Images.Count > 0)
            {
                if(t.GetValues().Images.Count > 1)
                {
                    var _imgs = new List<InstaImage>();
                    foreach(var img in t.GetValues().Images)
                    {
                        _imgs.Add(new InstaImage
                        {
                            URI = img.StandardResolution.Url,
                            Height = img.StandardResolution.Height,
                            Width = img.StandardResolution.Width
                        });
                    }; 

                    var result = await api.UploadPhotosAlbumAsync(_imgs.ToArray(), await Helper.ConstructCaptionTextAsync(t.GetValues().Text, t.Searches, api));

                    if (result.Succeeded)
                    {
                        t.TaskExecuted();
                        Helper.Log($"Task Executed: {t.Name} - {t.TaskType.ToString()} {Environment.NewLine} {JsonConvert.SerializeObject(result.Value)}");
                    }
                    else
                    {
                        Helper.Log(JsonConvert.SerializeObject(result.Info)); 
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

                    var result = await api.UploadPhotoAsync(_img, await Helper.ConstructCaptionTextAsync(t.GetValues().Text, t.Searches, api)); 
                    if (result.Succeeded)
                    {
                        t.TaskExecuted();
                        Helper.Log($"Task Executed: {t.Name} - {t.TaskType.ToString()} {Environment.NewLine} {JsonConvert.SerializeObject(result.Value)}");
                    }
                    else
                    {
                        Helper.Log(JsonConvert.SerializeObject(result.Info));
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

                var result = await api.UploadVideoAsync(_vid, _img, await Helper.ConstructCaptionTextAsync(t.GetValues().Text, t.Searches, api));

                if (result.Succeeded)
                {
                    t.TaskExecuted();
                    Helper.Log($"Task Executed: {t.Name} - {t.TaskType.ToString()} {Environment.NewLine} {JsonConvert.SerializeObject(result.Value)}");
                }
                else
                {
                    Helper.Log(JsonConvert.SerializeObject(result.Info));
                }
            }
        }

        private static async void RunDMTaskAsync(TaskModel t, IInstaApi api)
        {
            var users = new List<InstaUserShort>(); 
            
            // get essential search results 
            foreach(var search in t.Searches)
            {
                foreach(var inString in search.GetInStrings())
                {
                    var _temp = await api.SearchUsersAsync(inString);
                    if (_temp.Succeeded)
                    {
                        users.AddRange(_temp.Value.ToList());
                    }
                }
            }

            var filteredUsers = await Helper.GetFilteredUsersAsync(users, t.Searches, api, 5);

            // Executing
            if (filteredUsers.Count > 0)
            {
                foreach (var user in filteredUsers)
                {
                    if (!user.IsPrivate)
                    {
                        var result = await api.SendDirectMessage(user.UserName, "", await Helper.ConstructCaptionTextAsync(t.GetValues().Text, t.Searches, api));
                        if (result.Succeeded)
                        {
                            t.TaskExecuted();
                            Helper.Log($"Task Executed: {t.Name} - {t.TaskType.ToString()} {Environment.NewLine} {JsonConvert.SerializeObject(result.Value)}");
                        }
                        else
                        {
                            Helper.Log(JsonConvert.SerializeObject(result.Info));
                        }
                    }
                }

            }
        }

        private static async void RunCommentTaskAsync(TaskModel t, IInstaApi api)
        {
            List<InstaMedia> medias = new List<InstaMedia>();

            foreach(var search in t.Searches)
            {
                if (search.InUsers)
                {
                    var _tempList = new List<InstaUserShort>(); 

                    foreach(var inString in search.GetInStrings())
                    {
                        var _temp = await api.SearchUsersAsync(inString);
                        if (_temp.Succeeded)
                        {
                            _tempList.AddRange(_temp.Value); 
                        }
                    }

                    _tempList = await Helper.GetFilteredUsersAsync(_tempList, t.Searches, api, 10);

                    foreach(var user in _tempList)
                    {
                        var _userMedia = await api.GetUserMediaAsync(user.UserName, PaginationParameters.MaxPagesToLoad(5)); 
                        if (_userMedia.Succeeded)
                        {
                            medias.AddRange(_userMedia.Value); 
                        }
                    }
                }

                if (search.InPosts)
                {
                    var _temp = await api.GetUserTimelineFeedAsync(PaginationParameters.MaxPagesToLoad(5));
                    if (_temp.Succeeded)
                    {
                        medias.AddRange(_temp.Value.Medias.ToList()); 
                    }
                }

                var filteredMedia = await Helper.GetFilteredMediaAsync(medias, t.Searches, api, 5); 

                foreach(var media in filteredMedia)
                {
                    var result = await api.CommentMediaAsync(media.InstaIdentifier, await Helper.ConstructCaptionTextAsync(t.GetValues().Text, t.Searches, api));
                    if (result.Succeeded)
                    {
                        t.TaskExecuted();
                        Helper.Log($"Task Executed: {t.Name} - {t.TaskType.ToString()} {Environment.NewLine} {JsonConvert.SerializeObject(result.Value)}");
                    }
                    else
                    {
                        Helper.Log(JsonConvert.SerializeObject(result.Info)); 
                    }
                    
                }
            }
           
        }
    }
}

