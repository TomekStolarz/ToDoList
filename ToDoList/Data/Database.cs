using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Models;

namespace ToDoList.Data
{
    public class Database
    {

        readonly SQLiteAsyncConnection _database;

        public Database(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Request>().Wait();
        }

        public Task<List<Request>> GetRequestAsync()
        {
             return _database.Table<Request>()
                .Where(x => x.Date <= DateTime.UtcNow.Date)
                .Where(x => x.IsFinished==false)
                .OrderBy(x => x.Hour)
                .ToListAsync();

        }

        public Task<List<Request>> GetRequestsByDate(DateTime date)
        {
            return _database.Table<Request>().Where(x => x.Date == date.Date).OrderBy(x => x.Hour).ToListAsync();
        }

        public Task<Request> GetRequestsByTitle(string title, TimeSpan time)
        {
            var requ = _database.Table<Request>()
                .Where(x => x.Title == title)
                .Where(x => x.IsFinished == false)
                .Where(x => x.Hour == time)
                .FirstAsync();
            return requ;
        }

        public Task<int> SaveRequestAsync(Request request)
        {
            return _database.InsertAsync(request);
        }

        public Task<int> DeleteRequestAsync(Request request)
        {
            return _database.DeleteAsync(request);
           
        }

        public Task<int> EditRequestAsync(Request request)
        {
            return _database.UpdateAsync(request);
        }

        public Task<int> ClearingDatabase()
        {
            DateTime dat = DateTime.Now.Date.AddMonths(-1);
            return _database.Table<Request>().Where(x => x.Date <= dat).Where(x => x.IsFinished == true).DeleteAsync();
        }

    }
    
}
