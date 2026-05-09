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

        if (string.IsNullOrEmpty(userId))
            return View(0);

        var invitations = await meetingRepository.GetUserPendingInvitationsAsync(userId);

        return View(invitations.Count);
    }
}