using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.BudgetMetal.Services.Facebook
{
    public interface IFacebookClient
    {
        Task<Tuple<int, string>> PublishSimplePost(string postText);
        string PublishToFacebook(string postText, string pictureURL);
        Task<Tuple<int, string>> UploadPhoto(string photoURL);
        Task<Tuple<int, string>> UpdatePhotoWithPost(string postID, string postText);
    }
}
