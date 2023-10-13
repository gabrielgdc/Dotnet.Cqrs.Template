using System;
using MediatR;

namespace Cqrs.Template.Domain.Events;

public class Event : INotification
{
	public DateTime Timestamp { get; }

	protected Event()
	{
		Timestamp = DateTime.UtcNow;
	}
}
