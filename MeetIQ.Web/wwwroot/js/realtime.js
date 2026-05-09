const connection = new signalR.HubConnectionBuilder()
    .withUrl("/hubs/meetiq")
    .withAutomaticReconnect([0, 2000, 5000, 10000])
    .configureLogging(signalR.LogLevel.Warning)
    .build();

// ── Connection ─────────────────────────────────────────
async function startSignalR() {
    try {
        await connection.start();
    } catch {
        setTimeout(startSignalR, 5000);
    }
}

connection.onreconnected(() => {
    showToast({
        icon: "wifi", color: "text-emerald-400",
        title: "Reconnected", message: "Real-time connection restored"
    });
});

// ══════════════════════════════════════════════════════
//  INCOMING EVENTS
// ══════════════════════════════════════════════════════

connection.on("NewNotification", (n) => {
    prependToPanel(n);
    incrementBadge("notif-badge");

    if (n.type === 12) incrementInvitBadge();   // InvitationReceived
    if (n.type === 13) decrementInvitBadge();   // InvitationRevoked

    showToast(toastFromNotification(n));
});

connection.on("MeetingStarted", (payload) => {
    showToast({
        icon: "video", color: "text-emerald-400",
        title: "Meeting Started!",
        message: `"${payload.meetingTitle}" is now live`,
        action: { label: "Join Now", href: `/Meetings/Room/${payload.meetingId}` },
        duration: 10000
    });
});

connection.on("MeetingEnded", (payload) => {
    showToast({
        icon: "phone-off", color: "text-slate-400",
        title: "Meeting Ended",
        message: `"${payload.meetingTitle}" has ended`
    });
    if (window.location.pathname.toLowerCase().includes("/meetings/room"))
        setTimeout(() => window.location.href = `/Meetings/Details/${payload.meetingId}`, 2000);
});

connection.on("MeetingCancelled", (payload) => {
    showToast({
        icon: "x-circle", color: "text-red-400",
        title: "Meeting Cancelled",
        message: `"${payload.meetingTitle}" has been cancelled`
    });
});

// ══════════════════════════════════════════════════════
//  NOTIFICATION PANEL
// ══════════════════════════════════════════════════════

let panelLoaded = false;

function toggleNotifPanel() {
    const panel = document.getElementById("notif-panel");
    if (!panel) return;

    const isHidden = panel.classList.contains("hidden");
    panel.classList.toggle("hidden", !isHidden);

    if (isHidden && !panelLoaded) loadNotifications();
}

document.addEventListener("click", (e) => {
    const wrapper = document.getElementById("notif-wrapper");
    if (wrapper && !wrapper.contains(e.target))
        document.getElementById("notif-panel")?.classList.add("hidden");
});

async function loadNotifications() {
    try {
        const res = await fetch("/Notifications/Panel");
        const data = await res.json();
        renderList(data);
        panelLoaded = true;
    } catch {
        const loading = document.getElementById("notif-loading");
        if (loading) loading.textContent = "Failed to load.";
    }
}

function renderList(items) {
    const list = document.getElementById("notif-list");
    const loading = document.getElementById("notif-loading");
    if (loading) loading.remove();

    if (!items?.length) {
        list.innerHTML = `
            <div class="flex flex-col items-center justify-center py-10 text-center px-4">
                <i data-lucide="bell-off" class="w-8 h-8 text-slate-700 mb-2"></i>
                <p class="text-slate-600 text-xs">No notifications yet</p>
            </div>`;
        lucide.createIcons();
        return;
    }

    list.innerHTML = items.map(renderItem).join("");
    lucide.createIcons();
}

function prependToPanel(n) {
    if (!panelLoaded) return;
    const list = document.getElementById("notif-list");
    if (!list) return;

    const div = document.createElement("div");
    div.innerHTML = renderItem(n);
    list.prepend(div.firstElementChild);
    lucide.createIcons();
}

// ── Icon map ───────────────────────────────────────────
const ICON_MAP = {
    1: { icon: "calendar", color: "text-brand-400" },
    2: { icon: "clock", color: "text-amber-400" },
    3: { icon: "x-circle", color: "text-red-400" },
    4: { icon: "calendar", color: "text-blue-400" },
    5: { icon: "phone-off", color: "text-slate-400" },
    10: { icon: "user-plus", color: "text-emerald-400" },
    11: { icon: "video", color: "text-emerald-400" },
    12: { icon: "mail", color: "text-brand-400" },
    13: { icon: "mail-x", color: "text-slate-400" },
    14: { icon: "user-check", color: "text-emerald-400" },
    15: { icon: "user-x", color: "text-red-400" },
    20: { icon: "mic-2", color: "text-violet-400" },
    21: { icon: "sparkles", color: "text-violet-400" },
    22: { icon: "check-square", color: "text-emerald-400" },
    30: { icon: "check-square", color: "text-brand-400" },
    31: { icon: "clock", color: "text-amber-400" },
    32: { icon: "alert-circle", color: "text-red-400" },
    33: { icon: "refresh-cw", color: "text-slate-400" },
    40: { icon: "notebook-pen", color: "text-blue-400" },
    50: { icon: "flag", color: "text-amber-400" },
    51: { icon: "alert-triangle", color: "text-red-400" },
};

function renderItem(n) {
    const { icon, color } = ICON_MAP[n.type] ?? { icon: "bell", color: "text-slate-400" };
    const unread = !n.isRead;

    return `
        <div class="flex items-start gap-3 px-4 py-3.5 cursor-pointer
                    ${unread ? "bg-brand-600/5" : ""}
                    hover:bg-surface-hover transition-colors
                    border-b border-surface-border last:border-0"
             onclick="handleNotifClick('${n.id}', '${n.actionUrl ?? ""}', this)">
            <div class="w-7 h-7 rounded-lg bg-surface border border-surface-border
                        flex items-center justify-center shrink-0 mt-0.5">
                <i data-lucide="${icon}" class="w-3.5 h-3.5 ${color}"></i>
            </div>
            <div class="flex-1 min-w-0">
                <p class="text-xs font-semibold text-white leading-snug">${escHtml(n.title)}</p>
                <p class="text-xs text-slate-500 mt-0.5 leading-relaxed">${escHtml(n.message)}</p>
                <p class="text-[10px] text-slate-600 mt-1">${timeAgo(new Date(n.createdAt))}</p>
            </div>
            ${unread ? `<span class="w-1.5 h-1.5 rounded-full bg-brand-500 shrink-0 mt-2"></span>` : ""}
        </div>`;
}

async function handleNotifClick(id, url, el) {
    if (el.classList.contains("bg-brand-600/5")) {
        el.classList.remove("bg-brand-600/5");
        el.querySelector(".bg-brand-500")?.remove();
        decrementBadge("notif-badge");

        const csrf = document.querySelector("input[name='__RequestVerificationToken']")?.value ?? "";
        await fetch(`/Notifications/MarkAsRead/${id}`, {
            method: "POST",
            headers: { "RequestVerificationToken": csrf }
        });
    }
    if (url) window.location.href = url;
}

async function markAllRead() {
    const csrf = document.querySelector("input[name='__RequestVerificationToken']")?.value ?? "";
    await fetch("/Notifications/MarkAllRead", {
        method: "POST",
        headers: { "RequestVerificationToken": csrf }
    });

    document.querySelectorAll("#notif-list .bg-brand-600\\/5").forEach(el => {
        el.classList.remove("bg-brand-600/5");
        el.querySelector(".bg-brand-500")?.remove();
    });

    resetBadge("notif-badge");
}

// ══════════════════════════════════════════════════════
//  BADGE HELPERS
// ══════════════════════════════════════════════════════

function incrementBadge(id) {
    const b = document.getElementById(id);
    if (!b) return;
    const n = (parseInt(b.textContent) || 0) + 1;
    b.textContent = n > 99 ? "99+" : n;
    b.classList.remove("hidden");
}

function decrementBadge(id) {
    const b = document.getElementById(id);
    if (!b) return;
    const n = (parseInt(b.textContent) || 0) - 1;
    if (n <= 0) { b.classList.add("hidden"); b.textContent = ""; }
    else b.textContent = n;
}

function resetBadge(id) {
    const b = document.getElementById(id);
    if (!b) return;
    b.classList.add("hidden");
    b.textContent = "";
}

function incrementInvitBadge() {
    const b = document.getElementById("invitation-badge");
    if (!b) {
        const link = document.querySelector('[href="/Meetings/MyInvitations"]');
        if (!link) return;
        const s = document.createElement("span");
        s.id = "invitation-badge";
        s.className = "ml-auto bg-brand-600/20 text-brand-400 text-[11px] font-mono px-1.5 py-0.5 rounded-md";
        s.textContent = "1";
        link.appendChild(s);
    } else {
        b.textContent = (parseInt(b.textContent) || 0) + 1;
    }
}

function decrementInvitBadge() {
    const b = document.getElementById("invitation-badge");
    if (!b) return;
    const n = (parseInt(b.textContent) || 0) - 1;
    if (n <= 0) b.remove();
    else b.textContent = n;
}

// ══════════════════════════════════════════════════════
//  TOAST
// ══════════════════════════════════════════════════════

function toastFromNotification(n) {
    const { icon, color } = ICON_MAP[n.type] ?? { icon: "bell", color: "text-slate-400" };
    return {
        icon, color,
        title: n.title,
        message: n.message,
        action: n.actionUrl ? { label: "View", href: n.actionUrl } : null
    };
}

let toastQueue = [];
let toastActive = false;

function showToast({ icon, color, title, message, action, duration = 5000 }) {
    toastQueue.push({ icon, color, title, message, action, duration });
    if (!toastActive) nextToast();
}

function nextToast() {
    if (!toastQueue.length) { toastActive = false; return; }
    toastActive = true;
    const { icon, color, title, message, action, duration } = toastQueue.shift();

    const t = document.createElement("div");
    t.className = `fixed bottom-6 right-6 z-[9999] flex items-start gap-3 p-4 rounded-2xl max-w-sm w-full
                   bg-surface-card border border-surface-border shadow-2xl
                   transition-all duration-300 opacity-0 translate-y-2`;

    t.innerHTML = `
        <div class="w-8 h-8 rounded-xl bg-surface border border-surface-border
                    flex items-center justify-center shrink-0 mt-0.5">
            <i data-lucide="${icon}" class="w-4 h-4 ${color}"></i>
        </div>
        <div class="flex-1 min-w-0">
            <p class="text-sm font-semibold text-white">${escHtml(title)}</p>
            <p class="text-xs text-slate-500 mt-0.5 leading-relaxed">${escHtml(message)}</p>
            ${action ? `<a href="${action.href}"
                           class="inline-flex items-center gap-1 mt-2 text-xs font-semibold
                                  text-brand-400 hover:text-brand-300 transition-colors">
                            ${action.label}
                            <i data-lucide="arrow-right" class="w-3 h-3"></i>
                        </a>` : ""}
        </div>
        <button onclick="this.closest('div[class*=fixed]').remove(); toastActive=false; nextToast();"
                class="p-1 text-slate-600 hover:text-slate-400 transition-colors shrink-0">
            <i data-lucide="x" class="w-3.5 h-3.5"></i>
        </button>`;

    document.body.appendChild(t);
    lucide.createIcons();

    requestAnimationFrame(() => t.classList.remove("opacity-0", "translate-y-2"));

    setTimeout(() => {
        t.classList.add("opacity-0", "translate-y-2");
        setTimeout(() => { t.remove(); toastActive = false; nextToast(); }, 300);
    }, duration);
}

// ══════════════════════════════════════════════════════
//  HELPERS
// ══════════════════════════════════════════════════════

function timeAgo(date) {
    const s = Math.floor((Date.now() - date) / 1000);
    if (s < 60) return "Just now";
    if (s < 3600) return `${Math.floor(s / 60)}m ago`;
    if (s < 86400) return `${Math.floor(s / 3600)}h ago`;
    return `${Math.floor(s / 86400)}d ago`;
}

function escHtml(str) {
    return String(str ?? "")
        .replace(/&/g, "&amp;")
        .replace(/</g, "&lt;")
        .replace(/>/g, "&gt;")
        .replace(/"/g, "&quot;");
}

// ── Init ──────────────────────────────────────────────
startSignalR();