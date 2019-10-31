============================================================================================================================================
Below are the important developer instructions, to follow after installation of this package
============================================================================================================================================

1. Please fing the json configuration details below. You can override these using custom configurations which you can leverage using 
	package https://www.nuget.org/packages/PivotalServices.CloudFoundry.Replatform.Bootstrap.Configuration


	{
	  "management": {
		"endpoints": {
		  "path": "/cloudfoundryapplication",
		  "cloudfoundry": {
			"validateCertificates": false
		  }
		},
		"metrics": {
		  "exporter": {
			"cloudfoundry": {
			  "validateCertificates": false
			}
		  }
		}
	  }
	}

============================================================================================================================================
