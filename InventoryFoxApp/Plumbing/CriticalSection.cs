using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InventoryFoxApp.Plumbing
{
	/// <summary>
	/// This class performs non-queuing critcal section access gating. 
	/// When a thread calls EnterCriticalSection method, the delegate will be executed
	/// if and only if no other thread is already in the critical section. Each instance of 
	/// CriticalSection is treated independently
	/// </summary>
	public class CriticalSection
	{
		ManualResetEvent mre = new ManualResetEvent(true);

		public CriticalSection()
		{

		}

		/// <summary>
		/// Returns true if the critical section was entered
		/// Returns false if the critical section was skipped (another thread is currently in the critical section)
		/// </summary>
		public async Task<bool> EnterCriticalSectionAsync(Func<Task> asyncCallback)
		{
			bool result = await Task.Run<bool>(async () =>
			{
				if (!mre.WaitOne(0))
				{
					System.Diagnostics.Debug.WriteLine("Skipped excess event");
					return false;
				}

				try
				{
					mre.Reset();
					await asyncCallback();
					return true;
				}
				catch
				{
					throw;
				}
				finally
				{
					mre.Set();
				}
			});
			return result;
		}


		/// <summary>
		/// Returns true if the critical section was entered
		/// Returns false if the critical section was skipped (another thread is currently in the critical section)
		/// </summary>
		public async Task<bool> EnterCriticalSection(Action callback)
		{
			bool result = await Task.Run<bool>(async () =>
			{
				if (!mre.WaitOne(0))
				{
					System.Diagnostics.Debug.WriteLine("Skipped excess event");
					return false;
				}

				try
				{
					mre.Reset();
					callback.Invoke();
					return true;
				}
				catch
				{
					throw;
				}
				finally
				{
					mre.Set();
				}
			});
			return result;
		}
	}
}
