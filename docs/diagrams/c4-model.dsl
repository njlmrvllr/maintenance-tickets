workspace "Maintenance Tickets" "C4 model of the maintenance ticket management system." {

    !identifiers hierarchical

    model {
        user = person "User" "Employee who reports an incident on an equipment or facility (ATM, branch, office)."
        operator = person "Operator" "Maintenance staff who diagnoses the incident and moves the ticket through its workflow."

        maintenanceTickets = softwareSystem "Maintenance Tickets" "Registers incidents and manages each ticket through a validated status workflow, keeping the full history of changes." {
            webApp = container "Web application" "User view to report incidents. Operator board with filters, detail panel, history timeline and notification center." "Angular 20, Angular Material, Nginx" "Web Browser"
            api = container "REST API" "Applies the state machine rules and exposes /api/v1/tickets. Documented with Swagger." ".NET 10, ASP.NET Core, Dapper"
            database = container "Database" "tickets and ticket_history tables. One stored procedure per operation; status change and history saved in one transaction." "MySQL 8.4" "Database"
        }

        notificationChannel = softwareSystem "Notification channel" "Email or SMS to the reporter. Simulated in this version: the API writes the message to its log." "External"

        user -> maintenanceTickets.webApp "Reports incidents" "HTTPS"
        operator -> maintenanceTickets.webApp "Reviews and updates tickets" "HTTPS"
        maintenanceTickets.webApp -> maintenanceTickets.api "Calls" "JSON over HTTPS"
        maintenanceTickets.api -> maintenanceTickets.database "Calls stored procedures" "MySQL protocol, private network"
        maintenanceTickets.api -> notificationChannel "Sends a notification on every status change" "Simulated"
        notificationChannel -> user "Tells the reporter the new status"
    }

    views {
        systemContext maintenanceTickets "SystemContext" "System context (C4 level 1)" {
            include *
            autolayout lr
        }

        container maintenanceTickets "Containers" "Containers (C4 level 2)" {
            include *
            autolayout lr
        }

        styles {
            element "Element" {
                color #ffffff
            }
            element "Person" {
                shape person
                background #0a3d62
            }
            element "Software System" {
                background #0b74b8
            }
            element "Container" {
                background #3a8fd0
            }
            element "Web Browser" {
                shape WebBrowser
            }
            element "Database" {
                shape cylinder
            }
            element "External" {
                background #8a99a8
            }
        }
    }
}
