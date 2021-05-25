using System;
using System.Collections.Generic;
using System.Text;

namespace RodnaPamet.Services
{
	public class UploadBytesProgress
	{
		public UploadBytesProgress(string fileName, long bytesSent, long totalBytes)
		{
			Filename = fileName;
			BytesSent = bytesSent;
			TotalBytes = totalBytes;
		}

		public long TotalBytes { get; private set; }

		public long BytesSent { get; private set; }

		public float PercentComplete { get { return (float)BytesSent / TotalBytes; } }

		public string Filename { get; private set; }

		public bool IsFinished { get { return BytesSent >= TotalBytes; } }
	}
}
