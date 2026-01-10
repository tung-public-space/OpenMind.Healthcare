using MediatR;
using QuitSmokingApi.Features.Progress.Domain;

namespace QuitSmokingApi.Features.Progress.GetProgress;

public record GetProgressQuery : IRequest<UserProgress?>;
