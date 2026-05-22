using Angular.Dto;
using Core.Timeline;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Linq;
using Core.Extensions;
using Angular.Repository;
using Angular.Utils;

namespace Controllers
{
    [Authorize]
    [RoutePrefix("api/timeline")]
    public class ActivityTimelineController : ApplicationApiController
    {
        private const int NUMBER_OF_ACTIVITIES = 21;

        ActivityTimelineRepository<Activity> repository = new SqlActivityTimelineRepository();

        public ActivityTimelineController()
        {
            
        }

        [HttpGet]
        [Route("allActivities")]
        public IEnumerable<object> GetAllActivities(
        [ModelBinder(typeof(OrganizationModelBinding))] OrganizationId organizationId)
        {
            List<Activity> activities = repository.Get(organizationId.Value);
            return activities.Take(NUMBER_OF_ACTIVITIES);
        }

        [HttpGet]
        [Route("activityType/{activityType}")]
        public IHttpActionResult GetActivitiesByType(
        [ModelBinder(typeof(OrganizationModelBinding))] OrganizationId organizationId,
        string activityType)
        {
            if(activityType == null)
                return BadRequest("Type was null");

            List<Activity> activities = repository.Get(organizationId.Value, activityType);
            return Ok(activities.Take(NUMBER_OF_ACTIVITIES));
        }

        [HttpGet]
        [Route("reportType/{reportType}")]
        public IHttpActionResult GetActivitiesByReportType(
        [ModelBinder(typeof(OrganizationModelBinding))] OrganizationId organizationId,
        string reportType)
        {
            if (reportType == null)
                return BadRequest("Report type was null");

            var activities = repository.Get<ReportActivity>(organizationId.Value);
            activities = activities.Where(x => x.ReportType == reportType).ToList();
            return Ok(activities.Take(NUMBER_OF_ACTIVITIES));
        }

        [HttpGet]
        [Route("tag/{tag}")]
        public IHttpActionResult GetActivitiesByTag(
        [ModelBinder(typeof(OrganizationModelBinding))] OrganizationId organizationId,
        string tag)
        {
            if (tag == null)
                return BadRequest("Report type was null");

            List<Activity> activities = repository.GetByTag(organizationId.Value, tag).Take(NUMBER_OF_ACTIVITIES).ToList();
            return Ok(activities);
        }

        [HttpPost]
        [Route("postActivity")]
        public IHttpActionResult CreateNewOrganization([FromBody] Activity activity)
        {
            if (activity == null)
                return BadRequest("Activity was null");

            repository.Send(activity);
            return Ok();
        }

        [HttpGet]
        [Route("unfinishedByTypeAndDateRange")]
        public IHttpActionResult GetUnfinishedOrErrorActivitiesByReportTypeAndDateRange(
        [ModelBinder(typeof(OrganizationModelBinding))] OrganizationId organizationId,
        string reportType, DateTime dateFrom, DateTime dateTo)
        {
            if (reportType == null)
                return BadRequest("Report type was null");

            List<ReportActivity> activities = new List<ReportActivity>();
            activities = repository.GetUnfinishedAndErrorReportActivitiesByDateRange(organizationId.Value, dateFrom.AbsoluteStart(), dateTo.AbsoluteEnd());
            return Ok(activities);
        }
    }
}
