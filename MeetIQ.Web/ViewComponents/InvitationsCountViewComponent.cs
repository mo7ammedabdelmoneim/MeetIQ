using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using MeetIQ.Application.Interfaces.Repositories;

public class InvitationsCountViewComponent : ViewComponent
{
    private readonly IMeetingRepository meetingRepository;

    public InvitationsCountViewComponent(IMeetingRepository meetingRepository)
    {
        this.meetingRepository = meetingRepository;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var userId = UserClaimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId)) return Content(string.Empty);

        var invitations = await meetingRepository.GetUserPendingInvitationsAsync(userId);
        var count = invitations.Count;

        if (count == 0) return Content(string.Empty);

        return Content($@"<span class=""ml-auto text-[10px] font-bold px-1.5 py-0.5 rounded-full
                                     bg-brand-600 text-white"">{count}</span>");
    }
}