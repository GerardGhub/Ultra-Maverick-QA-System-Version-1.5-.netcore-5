import { Component, OnInit } from '@angular/core';
import { DashboardService } from 'src/app/dashboard.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit
{
Designation: string;
Username: string;
NoOfTeamMembers: number;
TotalCostOfAllProjects: number;
PendingTask: number;
UpcomingProjects: number;
ProjectCost: number;
CurrentExpenditure: number;
AvailableFunds: number;

Clients: string[];
Projects: string[];
Years: number[] = [];
TeamMembersSummary = [];
TeamMembers =[];
ToDay: Date;
  constructor(private dashboardService: DashboardService)
  {

  }

  ngOnInit()
  {
    this.Designation = "Team Leader";
    this.Username ="John Smith";
    this.NoOfTeamMembers = 67;
    this.TotalCostOfAllProjects = 240;
    this.PendingTask = 15;
    this.UpcomingProjects = 0.2;
    this.ProjectCost = 2113507;
    this.CurrentExpenditure = 96788;
    this.AvailableFunds = 52536;
    this.ToDay = new Date();

    this.Clients = [
      "ABC Infotech","React Man","Mabukokk"
    ];

    this.Projects = [
      "Project A1","Project B","Project C","Project D"
    ];

    for(var i = 2019; i >= 2010; i--)
    {
      this.Years.push(i);
    }

    this.TeamMembersSummary = this.dashboardService.getTeamMembersSummary();
    this.TeamMembers = [
      {
        Region: 'East',
        Members: [
          { ID: 1, Name: 'Ford', Status: 'Available' },
          { ID: 2, Name: 'Miller', Status: 'Available' },
          { ID: 3, Name: 'Jones', Status: 'Busy' },
          { ID: 4, Name: 'James', Status: 'Busy' },
        ],
      },
      {
        Region: 'West',
        Members: [
          { ID: 5, Name: 'Anna', Status: 'Available' },
          { ID: 6, Name: 'Arun', Status: 'Available' },
          { ID: 7, Name: 'Varun', Status: 'Busy' },
          { ID: 8, Name: 'Jasmine', Status: 'Busy' },
        ],
      },
      {
        Region: 'South',
        Members: [
          { ID: 9, Name: 'Krishna', Status: 'Available' },
          { ID: 10, Name: 'Mohan', Status: 'Available' },
          { ID: 11, Name: 'Raju', Status: 'Busy' },
          { ID: 12, Name: 'Farooq', Status: 'Busy' },
        ],
      },
      {
        Region: 'North',
        Members: [
          { ID: 13, Name: 'Jacob', Status: 'Available' },
          { ID: 14, Name: 'Smith', Status: 'Available' },
          { ID: 15, Name: 'Jones', Status: 'Busy' },
          { ID: 16, Name: 'James', Status: 'Busy' },
        ],
      },
    ];

  }

  onProjectChange($event)
  {
    // console.log($event.target.innerHTML);
    if($event.target.innerHTML == "Project A")
    {
      this.ProjectCost = 1;
      this.CurrentExpenditure =2;
      this.AvailableFunds = 3;
    }
    else if($event.target.innerHTML == "Project B")
    {
      this.ProjectCost = 1;
      this.CurrentExpenditure =22;
      this.AvailableFunds = 33;
    }
    else if($event.target.innerHTML == "Project C")
    {
      this.ProjectCost = 111;
      this.CurrentExpenditure =2;
      this.AvailableFunds = 113;
    }
    else if($event.target.innerHTML == "Project D")
    {
      this.ProjectCost = 101;
      this.CurrentExpenditure =200;
      this.AvailableFunds = 1-13;
    }
  }

}
