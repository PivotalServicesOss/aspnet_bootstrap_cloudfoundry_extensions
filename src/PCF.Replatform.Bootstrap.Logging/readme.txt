============================================================================================================================================
Below are the important developer instructions, to follow after installation of this package
============================================================================================================================================

1. Please fing the json configuration details below. You can override these using custom configurations which you can leverage using 
	package https://www.nuget.org/packages/PivotalServices.CloudFoundry.Replatform.Bootstrap.Configuration

	{
	  "management": {
		"tracing": {
		  "AlwaysSample": true,
		  "UseShortTraceIds": false,
		  "EgressIgnorePattern": "/api/v2/spans|/v2/apps/.*/permissions|/eureka/.*|/oauth/.*"
		}
	  },
	  "Serilog": {
		"MinimumLevel": {
		  "Default": "Information",
		  "Override": {
			"Microsoft": "Warning",
			"System": "Warning",
			"Pivotal": "Warning",
			"Steeltoe": "Warning"
		  }
		},
		"WriteTo": [
		  {
			"Name": "Console",
			"Args": {
			  "outputTemplate": "[{Level}]{CorrelationContext}=> RequestPath:{RequestPath} => {SourceContext} => {Message} {Exception}{NewLine}"
			}
		  },
		  {
			"Name": "Debug",
			"Args": {
			  "outputTemplate": "[{Level}]{CorrelationContext}=> RequestPath:{RequestPath} => {SourceContext} => {Message} {Exception}{NewLine}"
			}
		  }
		]
	  }
	}

============================================================================================================================================
