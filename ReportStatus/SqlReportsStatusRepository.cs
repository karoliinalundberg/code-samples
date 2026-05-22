using Angular.DAL;
using Angular.Models;
using Core.Reports;
using Core.ReportsStatus;
using Core.ReportsUploadStatus;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Repository
{
    public class SqlReportsStatusRepository : ReportsStatusRepository
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public SqlReportsStatusRepository()
        {

        }

        public void Save(ReportStatusCreated reportStatusItem)
        {
            ReportStatus reportStatus = MapToReportStatus(reportStatusItem);
            ReportStatus row = db.ReportsStatus.Find(reportStatus.OrganizationId, reportStatus.Occurred, reportStatus.ReportType);

            if (row == null)
            {

                db.ReportsStatus.Add(reportStatus);
            }
            else
            {

                row.TotalRows = reportStatus.TotalRows;
                row.Status = reportStatus.Status;
                db.Entry(row).State = EntityState.Modified;
            }

            db.SaveChanges();
        }

        public ReportStatusItem GetLatestReportStatusByReportTypeAndOrganizationId(string organizationId, string reportType)
        {
            ReportStatus reportStatus = db.ReportsStatus.Where(x => x.OrganizationId.Equals(organizationId) && x.ReportType.Equals(reportType)).OrderByDescending(y => y.Occurred).FirstOrDefault();

            if (reportStatus != null)
            {
                ReportStatusItem reportStatusItem = MapToReportStatusItem(reportStatus);
                return reportStatusItem;
            }

            return null;
        }

        public List<ReportStatusItem> GetAllReportsByReportTypeAndOrganizationId(string organizationId, string reportType)
        {
            List<ReportStatusItem> results = new List<ReportStatusItem>();
            List<ReportStatus> reports = db.ReportsStatus.Where(x => x.OrganizationId.Equals(organizationId) && x.ReportType.Equals(reportType)).ToList();

            if (reports.Count > 0)
            {
                foreach (var report in reports)
                {
                    results.Add(MapToReportStatusItem(report));
                }

                return results;
            }
            else
            {
                return null;
            }
        }

        private ReportStatus MapToReportStatus(ReportStatusCreated reportStatusItem)
        {
            ReportStatus reportStatus = new ReportStatus();

            reportStatus.OrganizationId = reportStatusItem.OrganizationId;
            reportStatus.Occurred = reportStatusItem.Occurred;
            reportStatus.DateFrom = reportStatusItem.DateFrom;
            reportStatus.DateTo = reportStatusItem.DateTo;
            reportStatus.ReportType = reportStatusItem.ReportType;
            reportStatus.Status = reportStatusItem.Status;
            reportStatus.TotalRows = reportStatusItem.TotalRows;

            return reportStatus;
        }

        private ReportStatusItem MapToReportStatusItem(ReportStatus reportStatus)
        {
            return new ReportStatusItem(reportStatus.OrganizationId, reportStatus.Occurred, reportStatus.DateFrom, reportStatus.DateTo,
                reportStatus.ReportType, reportStatus.Status, reportStatus.TotalRows);
        }
    }
}