using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetIQ.Domain.Enums
{
    public enum TranscriptStatus
    {
        PendingTranscription,   // audio uploaded, not yet transcribed
        Transcribing,           // Whisper job running
        Completed,              // text ready
        Failed
    }
}
