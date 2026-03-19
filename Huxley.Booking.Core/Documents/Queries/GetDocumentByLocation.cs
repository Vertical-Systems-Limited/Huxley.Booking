using FluentValidation;
using FluentValidation.Results;
using Huxley.Booking.Core.Documents.Data;
using Huxley.Core;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Huxley.Booking.Core.Documents.Queries;

public class GetDocumentByLocation(
    ILogger<GetDocumentByLocation> logger,
    IValidator<GetDocumentByLocationRequest> validator,
    IDocumentRepository documentRepository
    )
    : IRequestHandler<GetDocumentByLocationRequest, Result<GetDocumentByLocationResponse>>
{
    public async Task<Result<GetDocumentByLocationResponse>> Handle(GetDocumentByLocationRequest request, CancellationToken cancellationToken)
    {
        // validate - validator
        //
        var validation = await validator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
            return GetDocumentByLocationResponse.CreateFailure(validation.Errors.ToArray());

        try
        {
            // get the doc
            var document = await documentRepository.GetDocumentByReference(request.Location);
            // return
            return GetDocumentByLocationResponse.CreateSuccess(document);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed getting Document with reference {Location}", request.Location);
            return GetDocumentByLocationResponse.CreateFailure(e);
        }
    }
}

public class GetDocumentByLocationResponse
{
    public DocumentData Document { get; set; }
    public bool Successful { get; set; }
    public string[] FailureReasons { get; set; }

    private GetDocumentByLocationResponse(bool successful, string[] failureReasons, DocumentData document)
    {
        Successful = successful;
        FailureReasons = failureReasons;
        Document = document;
    }

    public static GetDocumentByLocationResponse CreateSuccess(DocumentData document) =>
        new GetDocumentByLocationResponse(true, [], document);

    public static GetDocumentByLocationResponse CreateFailure(string[] reasons) =>
        new GetDocumentByLocationResponse(false, reasons, null);

    public static GetDocumentByLocationResponse CreateFailure(string reason) =>
        new GetDocumentByLocationResponse(false, [reason], null);

    public static GetDocumentByLocationResponse CreateFailure(Exception ex) =>
        CreateFailure(ex.Message);

    public static GetDocumentByLocationResponse CreateFailure(ValidationFailure[] validationFailures) =>
        CreateFailure(validationFailures.Select(e => e.ErrorMessage).ToArray());

}

public class GetDocumentByLocationRequest : IRequest<Result<GetDocumentByLocationResponse>>
{
    public string Location { get; set; }
}

public class GetDocumentByLocationRequestValidator : AbstractValidator<GetDocumentByLocationRequest>
{
    public GetDocumentByLocationRequestValidator()
    {
    }
}