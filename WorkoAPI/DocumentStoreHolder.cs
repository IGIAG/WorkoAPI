using Raven.Client.Documents;
using Raven.Client.Util;
using System.Security.Cryptography.X509Certificates;

namespace WorkoAPI;


// 'DocumentStore' is a main-entry point for client API.
// It is responsible for managing and establishing connections
// between your application and RavenDB server/cluster
// and is capable of working with multiple databases at once.
// Due to it's nature, it is recommended to have only one
// singleton instance per application
public static class DocumentStoreHolder
{
    private static readonly Lazy<IDocumentStore> LazyStore =
        new (() =>
        {
            //If you get an exception here you need to import the certificate for the database. Get it from the DB owner.
            X509Certificate2 clientCertificate = new("./cert.pfx");

            var store = new DocumentStore
            {
                Urls = new[] { "https://a.free.sd-net.ravendb.cloud" },
                Database = "Worko",
                Certificate = clientCertificate,
                Conventions =
                {
                    MaxNumberOfRequestsPerSession = 10,
                    UseOptimisticConcurrency = true,
                    UseCompression = true
                }
            };

            return store.Initialize();
        });

    public static IDocumentStore Store => LazyStore.Value;
}
