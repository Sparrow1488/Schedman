using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using VkNet;
using VkNet.AudioBypassService.Extensions;
using VkNet.Enums.Filters;
using VkNet.Model;
using VkNet.Model.Attachments;
using VkNet.Model.RequestParams;

namespace AutoPostFullParty
{
    internal class Program
    {
        public static void Main()
        {
            Console.WriteLine("Start AutoPost[FP]...");
            var services = new ServiceCollection();
            services.AddAudioBypass();
            var api = new VkApi(services);
            api.Authorize(new ApiAuthParams {
                Login = "123",
                Password = "123",
            });

            var fpId = GetFullPartyGroup(api);
            var wall = api.Wall.Get(new WallGetParams()
            {
                Domain = "gay_club1488228"
            });

            // VkNet.Model.Attachments
            // 282086355 - Downloads album
            //var albums = api.Photo.GetAlbums(new PhotoGetAlbumsParams()
            //{
            //    OwnerId = -fpId
            //}).Where(album => album.Title.ToLower().Contains("downloads")).FirstOrDefault();
            //17447 - Edit post
            //var postId = CreatePost(api, fpId);

            //string file = @"C:\Users\Александр\Downloads\131-1315622_green-forest-fog-nature-trees-dawn-wallpaper-iphone.jpg";
            //UploadFile(api, fpId, file);
            var editPostId = EditPost(api, fpId, 17447);

            Console.WriteLine("Exit app");
        }

        private static long GetFullPartyGroup(VkApi api)
        {
            long result = -1;
            var userGroups = api.Groups.Get(new GroupsGetParams()
            {
                UserId = api.UserId,
            });
            var groupsId = new List<string>();
            foreach (var group in userGroups)
                groupsId.Add(group.Id.ToString());
            var readOnlyGroups = api.Groups.GetById(groupsId, "", new GroupsFields());
            var fp = readOnlyGroups.Where(group => group.Name.ToLower().Contains("full party")).FirstOrDefault();
            result = fp.Id;
            // gay_club1488228
            return result;
        }

        private static long EditPost(VkApi api, long groupId, long postIdInput)
        {
            // 17446
            //var photoUri = new Uri("https://sun9-20.userapi.com/impg/rG44N-O3J7ybfXy1u-T_QpMdiySwPjPP_u9XfA/rcsVyc4t9jw.jpg?size=763x1080&quality=96&sign=7efaab987f3acf7491678c71e50b4b19&type=album");
            
            string fileUrl = @"C:\Users\Александр\Downloads\d92ce2b56aa858f2766eb9296ef827bc.jpg";
            var photo = UploadWallPhoto(api, groupId, fileUrl);

            //var uploadedPhotos = api.Photo.GetById(new List<string>() { "457304034" });

            var attachments = new List<MediaAttachment>();
            attachments.Add(photo);
            //attachments.Add(new Photo()
            //{
            //    OwnerId = -groupId,
            //    Url = new Uri(fileId)
            //});

            var postId = api.Wall.Edit(new WallEditParams()
            {
                Signed = false,
                PostId = postIdInput,
                OwnerId = -groupId,
                Message = "Hello FullPartyAutoPost! че edit",
                Attachments = attachments
            });
            return postId;
        }

        private static long CreatePost(VkApi api, long groupId)
        {
            var postId = api.Wall.Post(new WallPostParams()
            {
                Signed = false,
                OwnerId = -groupId,
                Message = "Hello FullPartyAutoPost! че",
                FromGroup = true
                //Attachments = attachments
            });
            return postId;
        }

        private static Photo UploadWallPhoto(VkApi api, long groupId, string fileUrl)
        {
            var uploadServer = api.Photo.GetWallUploadServer(groupId);
            var wc = new WebClient();
            var responseFile = Encoding.ASCII.GetString(wc.UploadFile(uploadServer.UploadUrl, fileUrl));
            // Сохранить загруженный файл
            var photos = api.Photo.SaveWallPhoto(responseFile, (ulong)api.UserId, (ulong)groupId, "Че происходит");

            //new PhotoSaveParams
            //{
            //    SaveFileResponse = responseFile,
            //    AlbumId = 282086355,
            //    GroupId = -groupId
            //}
            // 457304034 - Photo Id
            return photos[0];
        }
    }
}
