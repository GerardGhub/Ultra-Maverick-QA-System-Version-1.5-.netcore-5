import { Injectable } from '@angular/core';

@Injectable()
export class DashboardService {

  getTeamMembersSummary():any[]
  {
    var TeamMembersSummary =[
      {Region:"East", TeamMembersCount: 20, TemporarilyUnavailableMembers: 4},
      {Region:"West", TeamMembersCount: 40, TemporarilyUnavailableMembers: 8},
      {Region:"South", TeamMembersCount: 50, TemporarilyUnavailableMembers: 1},
      {Region:"North", TeamMembersCount: 70, TemporarilyUnavailableMembers: 4},
    ];
    return TeamMembersSummary;
  }

}
