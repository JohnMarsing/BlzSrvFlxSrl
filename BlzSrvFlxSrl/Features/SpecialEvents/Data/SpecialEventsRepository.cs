using System.Data;
using Dapper;
using BlzSrvFlxSrl.Data;
using static BlzSrvFlxSrl.Features.SqlServer;
using BlzSrvFlxSrl.Features.SpecialEvents.Enums;
using BlzSrvFlxSrl.Features.SpecialEvents.Models;

namespace BlzSrvFlxSrl.Features.SpecialEvents.Data;

public interface ISpecialEventsRepository
{
	string BaseSqlDump { get; }

	Task<List<SpecialEvent>> GetEventsByDateRange(DateTimeOffset? dateBegin, DateTimeOffset? dateEnd);
	//Task<EditMarkdownVM> GetDescription(int id);

	// Commands
	Task<int> UpdateDescription(int id, string description);
	Task<(int NewId, int SprocReturnValue, string ReturnMsg)> CreateSpecialEvent(FormVM formVM);
	Task<(int SprocReturnValue, string ReturnMsg)> UpdateSpecialEvent(SpecialEvents.FormVM formVM);
	Task<int> RemoveSpecialEvent(int id);
}

public class SpecialEventsRepository : BaseRepositoryAsync, ISpecialEventsRepository
{
	public SpecialEventsRepository(IConfiguration config, ILogger<SpecialEventsRepository> logger) : base(config, logger)
	{
	}

	public string BaseSqlDump
	{
		get { return base.SqlDump; }
	}

	public async Task<(int NewId, int SprocReturnValue, string ReturnMsg)> CreateSpecialEvent(SpecialEvents.FormVM formVM)
	{
		base.Sql = "SpecialEvent.stpSpecialEventInsert";
		base.Parms = new DynamicParameters(new
		{
			DateTime = formVM.EventDate,
			ShowBeginDate = formVM.ShowBeginDate,
			ShowEndDate = formVM.ShowEndDate,
			SpecialEventTypeId = formVM.SpecialEventTypeId,
			Title = formVM.Title,
			SubTitle = formVM.SubTitle,
			Description = formVM.Description,
			ImageUrl = formVM.ImageUrl,
			WebsiteUrl = formVM.WebsiteUrl,
			WebsiteDescr = formVM.WebsiteDescr,
			YouTubeId = formVM.YouTubeId
		});

		base.Parms.Add("@NewId", dbType: DbType.Int32, direction: ParameterDirection.Output);
		base.Parms.Add(ReturnValueParm, dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

		int newId = 0;
		int sprocReturnValue = 0;
		string returnMsg = "";

		return await WithConnectionAsync(async connection =>
		{
			base.log.LogDebug($"Inside {nameof(SpecialEventsRepository)}!{nameof(CreateSpecialEvent)}, {nameof(formVM.Title)}; about to execute SPROC: {base.Sql}");
			var affectedrows = await connection.ExecuteAsync(sql: base.Sql, param: base.Parms, commandType: System.Data.CommandType.StoredProcedure);
			sprocReturnValue = base.Parms.Get<int>(ReturnValueName);
			int? x = base.Parms.Get<int?>("NewId");
			if (x == null)
			{
				if (sprocReturnValue == ReturnValueViolationInUniqueIndex)
				{
					returnMsg = $"Database call did not insert a new record because it caused a Unique Index Violation; registration.EMail: {formVM.Title}; ";
					base.log.LogWarning($"...returnMsg: {returnMsg}; {Environment.NewLine} {base.Sql}");
				}
				else
				{
					returnMsg = $"Database call failed; registration.EMail: {formVM.Title}; SprocReturnValue: {sprocReturnValue}";
					base.log.LogWarning($"...returnMsg: {returnMsg}; {Environment.NewLine} {base.Sql}");
				}
			}
			else
			{
				newId = int.TryParse(x.ToString(), out newId) ? newId : 0;
				returnMsg = $"Special Event created for {formVM.Title}; NewId={newId}";
				base.log.LogDebug($"...Return newId:{newId}");
			}
			return (newId, sprocReturnValue, returnMsg);
		});
	}

	public async Task<(int SprocReturnValue, string ReturnMsg)> UpdateSpecialEvent(SpecialEvents.FormVM formVM)
	{
		base.Sql = "SpecialEvent.stpSpecialEventUpdate";
		base.Parms = new DynamicParameters(new
		{
			Id = formVM.Id,
			DateTime = formVM.EventDate,
			ShowBeginDate = formVM.ShowBeginDate,
			ShowEndDate = formVM.ShowEndDate,
			SpecialEventTypeId = formVM.SpecialEventTypeId,
			Title = formVM.Title,
			SubTitle = formVM.SubTitle,
			Description = formVM.Description,
			ImageUrl = formVM.ImageUrl,
			WebsiteUrl = formVM.WebsiteUrl,
			WebsiteDescr = formVM.WebsiteDescr,
			YouTubeId = formVM.YouTubeId
		});

		base.Parms.Add(ReturnValueParm, dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

		int sprocReturnValue = 0;
		string returnMsg = "";

		return await WithConnectionAsync(async connection =>
		{
			base.log.LogDebug(string.Format("Inside {0}"
				, nameof(SpecialEventsRepository) + "!" + nameof(UpdateSpecialEvent)));

			var affectedrows = await connection.ExecuteAsync(sql: base.Sql, param: base.Parms, commandType: System.Data.CommandType.StoredProcedure);
			sprocReturnValue = base.Parms.Get<int>(ReturnValueName);
			
			returnMsg = $"Special Event updated for {formVM.Title}; Id={formVM.Id}";
			base.log.LogDebug(string.Format("...returnMsg: {0}", returnMsg));

		return (sprocReturnValue, returnMsg);
		});
	}

	public async Task<int> RemoveSpecialEvent(int id)
	{
		base.Parms = new DynamicParameters(new { Id = id });
		base.Sql = $"DELETE FROM SpecialEvent.Event WHERE Id=@Id";
		return await WithConnectionAsync(async connection =>
		{
			base.log.LogDebug(string.Format("...Deleting id: {0}", id));
			var affectedrows = await connection.ExecuteAsync(sql: base.Sql, param: base.Parms);
			return affectedrows;
		});
	}


	public async Task<EditMarkdownVM> GetDescription(int id)
	{
		base.Parms = new DynamicParameters(new { Id = id });

		base.Sql = $@"
--DECLARE @Id int=
SELECT Id, Title
, ISNULL(Description, '') AS Description -- MarkDig doesnt like nulls
FROM KeyDate.UpcomingEvent
WHERE Id = @Id
";
		return await WithConnectionAsync(async connection =>
		{
			var rows = await connection.QueryAsync<EditMarkdownVM>(sql: base.Sql, param: base.Parms);
			return rows.SingleOrDefault();
		});
	}

	public async Task<int> UpdateDescription(int id, string description)
	{
		base.Parms = new DynamicParameters(new { Id = id, Description = description });
		base.Sql = $"UPDATE KeyDate.UpcomingEvent SET Description = @Description WHERE Id=@Id; ";
		return await WithConnectionAsync(async connection =>
		{
			log.LogDebug($"base.Sql: {base.Sql}, base.Parms:{base.Parms}");
			var count = await connection.ExecuteAsync(sql: base.Sql, param: base.Parms);
			return count;
		});
	}

	//https://stackoverflow.com/questions/4331189/datetime-vs-datetimeoffset
	public async Task<List<SpecialEvent>> GetEventsByDateRange(DateTimeOffset? dateBegin, DateTimeOffset? dateEnd)
	{
		base.Parms = new DynamicParameters(new
		{
			DateBegin = dateBegin,
			DateEnd = dateEnd
		});

		base.Sql = $@"
--Description is modified because MarkDig doesn't like nulls
--DECLARE @DaysAhead int =100, @DaysPast int =-3
SELECT
  Id, EventDate
, SpecialEventTypeId
, DaysDiff, DaysDiffDescr
, Title, SubTitle, ImageUrl, WebsiteUrl, WebsiteDescr, YouTubeId
, ISNULL(Description, '') AS Description 
, ShowBeginDate, ShowEndDate
FROM SpecialEvent.vwSpecialEvent
WHERE EventDate >= @DateBegin AND EventDate <=  @DateEnd
ORDER BY EventDate
";
		return await WithConnectionAsync(async connection =>
		{
			var rows = await connection.QueryAsync<SpecialEvent>(sql: base.Sql, param: base.Parms);
			return rows.ToList();
		});
	}

	private string GetYearId(RelativeYearEnum relativeYear)
	{
		return relativeYear switch
		{
			RelativeYearEnum.Previous => "c.PreviousYear",
			RelativeYearEnum.Current => "c.CurrentYear",
			RelativeYearEnum.Next => "c.NextYear",
			RelativeYearEnum.None => "0",
			_ => "c.CurrentYear",
		};

	}

	
}
