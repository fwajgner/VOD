namespace VOD.Fixtures
{
    using Microsoft.Data.Sqlite;
    using Microsoft.EntityFrameworkCore;
    using System;
    using VOD.Infrastructure;

    public class SqLiteContextFactory : IDisposable
    {
        public SqliteConnection Connection { get; private set; }

        public VODContext CreateContext()
        {
            if (this.Connection == null)
            {
                this.Connection = new SqliteConnection("DataSource=:memory:");
            
                this.Connection.Open();

                DbContextOptions<VODContext> options = CreateOptions();
                using VODContext context = new VODContext(options);
                context.Database.EnsureCreated();
            }

            return new VODContext(CreateOptions());
        }

        private DbContextOptions<VODContext> CreateOptions()
        {
            return new DbContextOptionsBuilder<VODContext>()
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

        //override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
         ~SqLiteContextFactory()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // uncomment the following line if the finalizer is overridden above.
            GC.SuppressFinalize(this);
        }
        #endregion

    }
}
