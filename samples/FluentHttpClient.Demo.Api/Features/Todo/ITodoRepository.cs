﻿using Bogus;

namespace FluentHttpClient.Demo.Api.Features.Todo
{
    public interface ITodoRepository
    {
        Task<string> CreateAsync(TodoItemDto model);
        Task<IEnumerable<TodoItem>> GetAllAsync();
        Task<TodoItem> GetById(string id);
    }

    /// <summary>
    /// Todo repository
    /// </summary>
    public class TodoRepository : ITodoRepository
    {
        private readonly Dictionary<string, TodoItem> items = new();
        private readonly Faker _faker = new();

        /// <summary>
        /// Constructor
        /// </summary>
        public TodoRepository()
        {
            if (!items.Any())
            {
                for (int i = 0; i < _faker.Random.Int(100, 500); i++)
                {
                    var item = new TodoItem
                    {
                        Id = _faker.Random.Replace("###-??#"),
                        Title = $"Todo item: {i + 1}",
                        Description = _faker.Lorem.Sentence(10),
                        Completed = _faker.PickRandomParam(new[] { true, false })
                    };

                    items.Add(item.Id, item);
                }
            }
        }
        /// <summary>
        /// Creates todo item
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Task<string> CreateAsync(TodoItemDto model)
        {
            var id = _faker.Random.Replace("###-??#");

            items.Add(id, new TodoItem { Id = id, Title = model.Title, Description = model.Description, Completed = false });

            return Task.FromResult(id);
        }

        /// <summary>
        /// Gets all todo items
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<TodoItem>> GetAllAsync()
        {
            var values = items.Select(i => i.Value);
            return Task.FromResult(values);
        }

        /// <summary>
        /// Get single todo item for specified ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<TodoItem> GetById(string id)
        {
            if(!items.ContainsKey(id)) return Task.FromResult<TodoItem>(null);

            var item = items[id];

            return Task.FromResult(item);
        }
    }
}