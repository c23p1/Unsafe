using System.ComponentModel.DataAnnotations;

namespace Domain.Enums;

public enum ProcessStatus
{
	[Display(Name = "Создан")]
	Created,
	[Display(Name = "В очереди")]
	Queued,
	[Display(Name = "Обрабатывается")]
	Running,
	[Display(Name = "Успешно завершён")]
	Completed,
	[Display(Name = "Прерван")]
	Aborted
}