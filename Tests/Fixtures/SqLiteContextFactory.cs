namespace Tests.Fixtures
{
    using Context;
    using Microsoft.Data.Sqlite;
    using Microsoft.EntityFrameworkCore;
    using System;

    public class SqLiteContextFactory : IDisposable
    {
        public SqliteConnection Connection { get; private set; }

        public ApplicationDbContext CreateContext()
        {
            if (this.Connection == null)
            {
                this.Connection = new SqliteConnection("DataSource=:memory:");
            
                this.Connection.Open();

                DbContextOptions<ApplicationDbContext> options = CreateOptions();
                using (var context = new ApplicationDbContext(options))
                {
                    context.Database.EnsureCreated();
                }
            }

            return new ApplicationDbContext(CreateOptions());
        }

        private DbContextOptions<ApplicationDbContext> CreateOptions()
        {
            return new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(this.Connection).Options;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // dispose managed state (managed objects).
                    this.Connection.Dispose();                
                }

                // free unmanaged resources (unmanaged objects) and override a finalizer below.
                // set large fields to null.
                this.Connection = null;

                disposedValue = true;
            }
        }

        // override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~SqLiteFixture()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion

    }
}
