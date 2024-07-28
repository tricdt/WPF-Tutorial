﻿using StateMVVM.Models;

namespace StateMVVM.Services
{
    public class PostService
    {
        public Task<IEnumerable<Post>> GetAllPosts()
        {
            IEnumerable<Post> posts = new List<Post>()
            {
                new Post()
                {
                    Title = "SingletonSean",
                    Content = "Is awesome",
                    DateCreated = new DateTime(2005, 12, 12)
                },
                new Post()
                {
                    Title = "SingletonSean2",
                    Content = "Wow",
                    DateCreated = new DateTime(2008, 12, 12)
                },
                new Post()
                {
                    Title = "hello world",
                    Content = "123",
                    DateCreated = new DateTime(2003, 12, 12)
                },
            };

            return Task.FromResult(posts);
        }
    }
}
