using MediatR;
using Microsoft.AspNetCore.Http;
using IResult = Assignment.Application.Models.IResult;

namespace Assignment.Application.Trial.Requests;

public record UploadFileRequest(IFormFile File) : IRequest<IResult>;