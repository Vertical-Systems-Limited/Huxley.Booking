using Huxley.Core.LegacyHelpers;
using Huxley.Hosting.Core;
using Huxley.Legacy.VslRuntime.Client;
using Microsoft.Extensions.Logging;

namespace Huxley.Booking.Core.Documents.Data;

public interface IDocumentRepository
{
    Task<IEnumerable<Document>> GetDocumentsByFileNo(string fileNo);
    Task<DocumentData> GetDocumentByReference(string reference);
}

public class DocumentRepository(
    ILogger<DocumentRepository> logger,
    IHuxleyVslRuntimeApiClient client,
    HuxleyContext context
) : IDocumentRepository
{
    public async Task<IEnumerable<Document>> GetDocumentsByFileNo(string fileNo)
    {
        try
        {
            // get the documents from the runtime
            var inputXml = $"<ROOT><BOOKING_REFERENCE>{fileNo}</BOOKING_REFERENCE></ROOT>";
            var result = await client.ExecuteRunFile(
                context.Tenant.TarscServerAddress,
                context.Tenant.TarscServerPort,
                "TARSCWS_V6\\-LETTLST.RUN",
                inputXml
            );
            
            // check for errors
            if(result.ErrorOccurred)
                throw new Exception("Error occurred while fetching documents: " + result.Message);

            // deserialize the output xml
            var documentRecords = result.OutputXml.DeserializeXmlToEnumerableOf<DocumentRecord>();
            
            // map to documents and return
            return documentRecords.Select(p => p.ToDocument()).ToList();
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to get documents by file number {FileNo}", fileNo);
            throw;
        }
    }

    public async Task<DocumentData> GetDocumentByReference(string location)
    {
        try
        {
            // get the document from the runtime
            var result = await client.GetArchiveFileFromServer(
                context.Tenant.TarscServerAddress, 
                context.Tenant.TarscServerPort, 
                location);
            
            // check for errors
            if(!result.Success)
                throw new Exception("Error occurred while fetching document: " + result.Error);
            
            // map to documents and return
            return new DocumentData()
            {
                Contents = result.Content
            };
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to get document by location {Location}", location);
            throw;
        }
    }
}