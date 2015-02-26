using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using Common.Logging;

namespace AzureTableStorageMagic.Infrastructure
{
    public class AzureStorageEmulator : IAzureStorageEmulator
    {
        private readonly TimeSpan _sleepAfterCallingProcess;
        private static readonly ILog Log = LogManager.GetLogger(typeof(AzureStorageEmulator));

        public AzureStorageEmulator()
            : this(TimeSpan.FromMilliseconds(50))   // Magic sleep number is required by at least 'when Azure service is not available' scenario in 'Repository.Add' feature.
        {
        }

        public AzureStorageEmulator(TimeSpan sleepAfterCallingProcess)
        {
            _sleepAfterCallingProcess = sleepAfterCallingProcess;
        }

        public const string ConnectionString = "UseDevelopmentStorage=true;";

        public bool IsEmulatorRunning()
        {
            var request = WebRequest.Create("http://127.0.0.1:10002/");

            try
            {
                request.GetResponse();
                return true;
            }
            catch (WebException exception)
            {
                switch (exception.Status)
                {
                    case WebExceptionStatus.ProtocolError:
                        return true;

                    case WebExceptionStatus.ConnectFailure:
                        return false;

                    default:
                        throw new Exception(string.Format("Expected status to be {0} or {1} but found {2}.", WebExceptionStatus.ProtocolError, WebExceptionStatus.ConnectFailure, exception.Status));
                }
            }
        }

        public void StartEmulatorIfNotRunning()
        {
            if (IsEmulatorRunning())
            {
                Log.Trace("Windows Azure Storage Emulator is running.");
                return;
            }

            Log.Trace("Starting Windows Azure Storage Emulator...");

            using (var process = Process.Start(CreateProcessStartInfo("start")))
            {
                if (process == null)
                {
                    throw new Exception("Cannot start Windows Azure Storage Emulator.");
                }
            }

            Log.TraceFormat("Sleeping {0:N0}ms.", _sleepAfterCallingProcess.TotalMilliseconds);
            Thread.Sleep(_sleepAfterCallingProcess);

            Log.Trace("Started Windows Azure Storage Emulator.");
        }

        public void StopEmulatorIfIsRunning()
        {
            if (!IsEmulatorRunning())
            {
                Log.Trace("Windows Azure Storage Emulator is not running.");
                return;
            }

            Log.Trace("Stopping Windows Azure Storage Emulator...");

            using (var process = Process.Start(CreateProcessStartInfo("stop")))
            {
                if (process == null)
                {
                    throw new Exception("Cannot stop Windows Azure Storage Emulator.");
                }
            }

            Log.TraceFormat("Sleeping {0:N0}ms.", _sleepAfterCallingProcess.TotalMilliseconds);
            Thread.Sleep(_sleepAfterCallingProcess);

            Log.Trace("Stopped Windows Azure Storage Emulator.");
        }

        private static ProcessStartInfo CreateProcessStartInfo(string arguments)
        {
            return new ProcessStartInfo
            {
                FileName = GetWAStorageEmulatorFileName(),
                Arguments = arguments,
                WindowStyle = ProcessWindowStyle.Hidden
            };
        }

        // ReSharper disable once InconsistentNaming
        private static string GetWAStorageEmulatorFileName()
        {
            var possibleDirectories = new List<string>(new[]
            {
                @"C:\Program Files (x86)\Microsoft SDKs\Azure\Storage Emulator",
                @"C:\Program Files\Microsoft SDKs\Azure\Storage Emulator",
                @"C:\Program Files (x86)\Microsoft SDKs\Windows Azure\Storage Emulator",
                @"C:\Program Files\Microsoft SDKs\Windows Azure\Storage Emulator"
            });

            var possibleFileNames = possibleDirectories.Select(possibleDirectory => Path.Combine(possibleDirectory, "WAStorageEmulator.exe"));
            var fileName = possibleFileNames.FirstOrDefault(File.Exists);

            if (fileName != null)
            {
                return fileName;
            }

            var message = string.Format(
                "Cannot find WAStorageEmulator.exe in {0}.",
                string.Join(" or ", possibleDirectories));

            throw new FileNotFoundException(message);
        }
    }
}