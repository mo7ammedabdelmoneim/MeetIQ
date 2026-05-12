"""
transcribe.py — MeetIQ Whisper Transcription Script
Usage: python transcribe.py <audio_file_path> [--model base]

Returns JSON to stdout:
  { "text": "...", "language": "en", "duration": 123.4 }
  or on error:
  { "error": "..." }
"""

import sys
import json
import os
import time
import argparse


def transcribe(audio_path: str, model_name: str) -> dict:
    # Validate file exists
    if not os.path.exists(audio_path):
        return {"error": f"File not found: {audio_path}"}

    try:
        import whisper
    except ImportError:
        return {"error": "Whisper not installed. Run: pip install openai-whisper"}

    try:
        # Load model (cached after first download)
        model = whisper.load_model(model_name)

        start = time.time()

        # transcribe — fp16=False for CPU (most dev machines have no GPU)
        result = model.transcribe(
            audio_path,
            fp16=False,
            verbose=False
        )

        elapsed = round(time.time() - start, 2)

        return {
            "text":     result.get("text", "").strip(),
            "language": result.get("language", "unknown"),
            "duration": elapsed
        }

    except Exception as e:
        return {"error": str(e)}


def main():
    parser = argparse.ArgumentParser()
    parser.add_argument("audio_path", help="Path to the audio file")
    parser.add_argument(
        "--model",
        default="base",
        choices=["tiny", "base", "small", "medium"],
        help="Whisper model size (default: base)"
    )
    args = parser.parse_args()

    result = transcribe(args.audio_path, args.model)

    # Print ONLY JSON to stdout — ASP.NET reads this
    print(json.dumps(result, ensure_ascii=False))


if __name__ == "__main__":
    main()