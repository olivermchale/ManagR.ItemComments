using ItemComments.Models.ViewModels;
using ItemComments.Services.Interfaces;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ItemComments.Services
{
    public class CommenterService : ICommenterService
    {
        private readonly HttpClient _client;
        public CommenterService(IHttpClientFactory clientFactory)
        {
            _client = clientFactory.CreateClient("accounts");
        }
        public async Task<bool> GetCommenter(CommentVm comment)
        {
            try
            {
                using (HttpResponseMessage response = await _client.GetAsync("/users/getUsername?id=" + comment.CommenterId))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var name = await response.Content.ReadAsStringAsync();
                        comment.CommenterName = name.Trim('"');
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                //excpetion getting commenter
            }
            return false;
        }
    }
}
