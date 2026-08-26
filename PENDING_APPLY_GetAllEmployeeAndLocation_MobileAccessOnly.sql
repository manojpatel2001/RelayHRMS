-- ============================================================================
-- Relaxes the "Location Assign to Employee" picker's eligibility filter.
-- Originally filtered on IsMobileAccess=1 AND IsGeofencingRequired=1, but
-- that's backwards: Geo Fencing gets turned on BY assigning a zone here, so
-- requiring it up front meant nobody could ever show up in the picker.
-- Correct rule: show every company employee with Mobile Access on, full
-- stop. (Matches the corresponding C# change in
-- HRMS_Infrastructure/Repository/OtherMaster/GeoLocationRepository.cs ->
-- GetAllEmployeeAndLocation(), which now filters on IsMobileAccess alone.)
-- ============================================================================

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

ALTER proc GetAllEmployeeAndLocation
@CompanyId int
as
begin

select Id ,EmployeeCode,FullName from aspnetusers where isdeleted=0 and isEnabled=1
and isnull(isleft,0) =0
and CompanyId=@CompanyId
and isnull(IsMobileAccess,0)=1
----------------------------
select
  g.*,
  b.BranchName as GeoLocationName
  from GeoLocation g
  left join Branch b on b.BranchId=g.BranchId
  where g.isdeleted=0 and g.isEnabled=1
  and g.CompanyId=@CompanyId

end
GO
