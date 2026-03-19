using FluentValidation;
using FluentValidation.Results;
using Huxley.Booking.Core.Documents.Data;
using Huxley.Core;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Huxley.Booking.Core.Documents.Queries;

public class GetDocumentsForBooking(
    ILogger<GetDocumentsForBooking> logger,
    IValidator<GetDocumentsForBookingRequest> validator,
    IDocumentRepository documentRepository
    )
    : IRequestHandler<GetDocumentsForBookingRequest, Result<GetDocumentsForBookingResponse>>
{
    public async Task<Result<GetDocumentsForBookingResponse>> Handle(GetDocumentsForBookingRequest request, CancellationToken cancellationToken)
    {
        // validate - validator
        //
        var validation = await validator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
            return GetDocumentsForBookingResponse.CreateFailure(validation.Errors.ToArray());

        try
        {
            // get the docs
            var docsList = await documentRepository.GetDocumentsByFileNo(request.FileNumber);
            // return
            return GetDocumentsForBookingResponse.CreateSuccess(docsList);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed getting Documents for booking with file number {FileNo}", request.FileNumber);
            return GetDocumentsForBookingResponse.CreateFailure(e);
        }
    }
}

public class GetDocumentsForBookingResponse
{
    public Document[] Documents { get; set; }
    public bool Successful { get; set; }
    public string[] FailureReasons { get; set; }

    private GetDocumentsForBookingResponse(bool successful, string[] failureReasons, Document[] documents)
    {
        Successful = successful;
        FailureReasons = failureReasons;
        Documents = documents;
    }

    public static GetDocumentsForBookingResponse CreateSuccess(IEnumerable<Document> documents) =>
        new GetDocumentsForBookingResponse(true, [], documents.ToArray());

    public static GetDocumentsForBookingResponse CreateFailure(string[] reasons) =>
        new GetDocumentsForBookingResponse(false, reasons, []);

    public static GetDocumentsForBookingResponse CreateFailure(string reason) =>
        new GetDocumentsForBookingResponse(false, [reason], []);

    public static GetDocumentsForBookingResponse CreateFailure(Exception ex) =>
        CreateFailure(ex.Message);

    public static GetDocumentsForBookingResponse CreateFailure(ValidationFailure[] validationFailures) =>
        CreateFailure(validationFailures.Select(e => e.ErrorMessage).ToArray());

}

public class GetDocumentsForBookingRequest : IRequest<Result<GetDocumentsForBookingResponse>>
{
    public string FileNumber { get; set; }
}

public class GetDocumentsForBookingRequestValidator : AbstractValidator<GetDocumentsForBookingRequest>
{
    public GetDocumentsForBookingRequestValidator()
    {
    }
}