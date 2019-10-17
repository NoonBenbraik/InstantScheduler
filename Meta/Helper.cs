using InstantScheduler.Models;
using InstaSharper.API;
using InstaSharper.Classes;
using InstaSharper.Classes.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstantScheduler.Meta
{
    public static class Helper
    {
        public static void Log(string text)
        {
            string directory = @"Meta\log\";
            string fileName = @"logger.txt";

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            if (!File.Exists(directory + fileName))
                File.Create(directory + fileName).Close();


            try
            {
                File.OpenWrite(directory + fileName); 
                File.AppendAllText(directory + fileName, $"{DateTime.Now}:  {text}");
            }
            catch
            {
                return;
            }


        }

        public static string PropsToString(object o)
        {
            string temp = "";
            o.GetType().GetProperties().ToList().ForEach(p =>
            {
                temp += p.Name + ": " + p.GetValue(o) + "\n";
            });

            return temp;
        }

        public static async Task<List<InstaMedia>> GetFilteredMediaAsync(List<InstaMedia> medias, List<SearchModel> searches, IInstaApi api, int count)
        {
            var FilteredMedia = new List<InstaMedia>();

            // Search for our results
            // In strings
            foreach(var search in searches)
            {
                if (search.InUsers)
                {
                    var _tempList = new List<InstaMedia>();

                    search.GetInStrings().ForEach(inString =>
                    {
                        _tempList.AddRange(medias.Where(m => (m.User.UserName + m.User.FullName).Contains(inString)));
                    });

                    search.GetExStrings().ForEach(exString =>
                    {
                        _tempList.RemoveAll(m => (m.User.UserName + m.User.FullName).Contains(exString));
                    });

                    search.GetInLocations().ForEach(inLocation =>
                    {
                        _tempList.AddRange(medias.Where(m =>
                        {
                            if (m.Location != null && inLocation.FbLocation != null)
                                return m.Location.FacebookPlacesId == inLocation.FbLocation.Pk;
                            else
                                return false;
                        }));
                    });

                    search.GetExLocations().ForEach(exLocation =>
                    {
                        _tempList.RemoveAll(m =>
                        {
                            if (m.Location != null && exLocation.FbLocation != null)
                                return m.Location.FacebookPlacesId == exLocation.FbLocation.Pk;
                            else
                                return false;
                        });
                    });

                    FilteredMedia.AddRange(_tempList);
                }

                if (search.InHashtags)
                {
                    var _tempList = new List<InstaMedia>();


                    search.GetInStrings().ForEach(inString =>
                    {
                        if (inString.StartsWith("#"))
                        {
                            _tempList.AddRange(medias.Where(m =>
                            {
                                if (m.Caption != null)
                                    return m.Caption.Text.Contains(inString);
                                else
                                    return false;
                            }));
                        }
                    });

                    search.GetExStrings().ForEach(exString =>
                    {
                        if (exString.StartsWith("#"))
                        {
                            _tempList.RemoveAll(m =>
                            {
                                if (m.Caption != null)
                                    return m.Caption.Text.Contains(exString);
                                else
                                    return false;
                            });
                        }
                    });

                    FilteredMedia.AddRange(_tempList);
                }

                if (search.InFollowingMe)
                {
                    var _tempMedia = new List<InstaMedia>();

                    foreach (var m in medias)
                    {
                        var isFollowingMe = await api.GetFriendshipStatusAsync(m.User.Pk);

                        if (isFollowingMe.Succeeded)
                            if (isFollowingMe.Value.FollowedBy)
                                _tempMedia.Add(m);
                    }

                    FilteredMedia.AddRange(_tempMedia);
                }

                if (search.InPosts)
                {
                    
                    // It a Must in Posts
                }
            };

            
            return FilteredMedia.Take(count).ToList(); 
        }

        public static async Task<List<InstaUserShort>> GetFilteredUsersAsync(List<InstaUserShort> users, List<SearchModel> searches, IInstaApi api, int count)
        {
            var FilteredUsers = new List<InstaUserShort>();

            // Search for our results
            // In strings
            foreach(var search in searches)
            {
                if (search.InUsers)
                {
                    var _tempList = new List<InstaUserShort>();

                    search.GetInStrings().ForEach(inString =>
                    {
                        _tempList.AddRange(users.Where(u => (u.UserName + u.FullName).Contains(inString)));
                    });

                    search.GetExStrings().ForEach(exString =>
                    {
                        _tempList.RemoveAll(m => (m.UserName + m.FullName).Contains(exString));
                    });

                    FilteredUsers.AddRange(_tempList);
                }

                if (search.InHashtags)
                {
                    // No way to implement
                }

                if (search.InFollowingMe)
                {
                    var _tempList = new List<InstaUserShort>();

                    foreach (var u in users)
                    {
                        var isFollowingMe = await api.GetFriendshipStatusAsync(u.Pk);

                        if (isFollowingMe.Succeeded)
                            if (isFollowingMe.Value.FollowedBy)
                                _tempList.Add(u);
                    }

                    FilteredUsers = _tempList;
                }

                if (search.InPosts)
                {
                    // No way to implement
                }
            };


            return FilteredUsers.Take(count).ToList();
        }

        public static async Task<string> ConstructCaptionTextAsync(string _text, List<SearchModel> searches, IInstaApi api)
        {
            var text = _text + Environment.NewLine; 

            foreach(var search in searches)
            {



                if (search.InHashtags)
                {
                    foreach(var inString in search.GetInStrings())
                    {
                        if (inString.StartsWith("#") && inString.Length > 1)
                        {
                            var _hashtag = await api.SearchHashtag(inString.Substring(1, inString.Length - 1));
                            if (_hashtag.Succeeded)
                            {
                                if(_hashtag.Value.Count > 0)
                                {
                                    var hashtag = _hashtag.Value.First(_h => _h.MediaCount == _hashtag.Value.Max(h => h.MediaCount));

                                    text += $"#{hashtag.Name} "; 
                                }
                            }
                        }
                    }
                }

                text += Environment.NewLine;

                if (search.InUsers)
                {
                    foreach (var inString in search.GetInStrings())
                    {
                        if (inString.StartsWith("@") && inString.Length > 1)
                        {
                            var _user = await api.SearchUsersAsync(inString.Substring(1, inString.Length - 1));
                            if (_user.Succeeded)
                            {
                                if (_user.Value.Count > 0)
                                {
                                    text += $"@{_user.Value.First().UserName} ";
                                }
                            }
                        }
                    }
                }
            }


            return text; 
        }




    }
}
