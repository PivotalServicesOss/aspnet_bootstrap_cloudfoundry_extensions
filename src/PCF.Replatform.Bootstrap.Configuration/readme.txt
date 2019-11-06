============================================================================================================================================
IMPORTANT NOTE: You can very well make use of the cf supply buildpack https://github.com/cloudfoundry-community/web-config-transform-buildpack 
				for performing token replaccement, transformation, etc.
============================================================================================================================================
============================================================================================================================================
Below are the important developer instructions, to follow after installation of this package
============================================================================================================================================

1. Please find the json configuration details below. You can override these using custom configurations like environment variable or even json.

	{
	  "spring": {
		"application": {
		  "name": "${vcap:application:name}"
		},
		"cloud": {
		  "config": {
			"validate_certificates": false,
			"failFast": true,
			"name": "${vcap:application:name}"
			"env": "${ASPNETCORE_ENVIRONMENT}"
		  }
		}
	  }
	},
	"AppSettings": {
	"Key1": "value1"
	},
	"ConnectionStrings": {
	"Database1": "connection1"
	},
	"Providers": {
	"Database1": "provider1"
	}

2. Follow the instructions here to setup config server https://pivotal.io/application-transformation-recipes/app-architecture/setting-up-spring-config-server
============================================================================================================================================
