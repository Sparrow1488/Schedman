﻿using ScheduleVkManager.Entities;
using ScheduleVkManager.Storage;
using ScheduleVkManager.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ScheduleVkManager
{
    public class Program
    {
        private static Logger _logger = new Logger();
        private static FileStorage _storage = new FileStorage();

        public static void Main()
        {
            _logger.Log("Started SheduleVkManager");

            var albumHandler = new AuthorsHandler();
            var mainDir = _logger.Input("Input album path");
            mainDir = @"C:\Users\aleks\OneDrive\Desktop\Илья\Repositories\Testable";
            var albums = new List<Album>();
            foreach (var authorDir in Directory.GetDirectories(mainDir)) {
                albums.Add(albumHandler.GetAlbum(authorDir));
            }

            var first = albums.First();
            first.UploadStatus = UploadStatus.Uploaded;
            ((List<AlbumItem>)first.Items).ForEach(item => item.UploadStatus = UploadStatus.Uploaded);
            string key = _storage.SaveAlbum(first);
            var storedAlbum = _storage.GetAlbum(key);

            #region VkManager

            //Task.Run(async () =>
            //{
            //    var authDataPath = @"C:\Users\Александр\Downloads\need_delete\delete_pas_log.txt";
            //    var authData = new AuthorizeData(authDataPath);
            //    var vkManager = new VkManager();
            //    var authResult = await vkManager.AuthorizeAsync(authData);
            //    if(!(authResult is true)) {
            //        PrintErrors(vkManager.Errors);
            //        vkManager.ClearErrors();
            //    }
            //    else _logger.Success("Authorize success");

            //    var group = await vkManager.GetGroupManagerAsync("full party");
            //    if(group.Id == 0) {
            //        PrintErrors(vkManager.Errors);
            //    }
            //    else _logger.Success("Success found group id_" + group.Id);

            //    var post = CreatePost();
            //    post = await group.AddPostAsync(post);
            //    if(post.Id == 0) {
            //        _logger.Error("Cannot create post on wall");
            //    }
            //    else _logger.Success("Success created post on wall id_" + post.Id);
            //    PrintPostInfo(post);

            //}).GetAwaiter().GetResult();
            #endregion
        }

        private static IEnumerable<string> GetRandomPhotosUrl()
        {
            var result = new List<string>();
            string mainDirectory = @"C:\Users\aleks\OneDrive\Desktop\Илья\Repositories\Testable";
            var filesInDir = Directory.GetFiles(mainDirectory);
            var rnd = new Random();
            result.Add(filesInDir[rnd.Next(0, filesInDir.Length - 1)]);
            result.Add(filesInDir[rnd.Next(0, filesInDir.Length - 1)]);
            result.Add(filesInDir[rnd.Next(0, filesInDir.Length - 1)]);

            return result;
        }

        private static CreatePost CreatePost()
        {
            var result = new CreatePost();
            _logger.Log("Let's create new post on wall!");
            result.Message = _logger.Input("Message");
            var photos = _logger.Input("Photos url (\"rnd\"if upload random)");
            if(photos == "rnd")
                result.PhotosUrl = GetRandomPhotosUrl();
            var date = _logger.Input("Schedule time (time today, use \":\")").Split(':');
            if(date.Length > 0) { 
                result.Schedule = new DateTime(DateTime.Now.Year,
                                       DateTime.Now.Month,
                                        DateTime.Now.Day,
                                         int.Parse(date[0]),
                                          int.Parse(date[1]),
                                           0);
                _logger.Log($"Set schedule on {result.Schedule}");
            }

            return result;
        }

        private static void PrintPostInfo(CreatePost post)
        {
            _logger.Log($"[id_{post.Id}] Post info\nMessage: {post.Message}\nPhotos: {post.PhotosUrl.Count()}\nSchedule: {post.Schedule}");
        }

        private static void PrintErrors(IEnumerable<string> errors)
        {
            foreach (var error in errors) {
                _logger.Error(error);
            }
        }
    }
}
