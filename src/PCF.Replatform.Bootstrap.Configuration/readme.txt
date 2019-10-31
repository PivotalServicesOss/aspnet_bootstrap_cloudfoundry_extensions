============================================================================================================================================
IMPORTANT NOTE: You can very well make use of the cf supply buildpack https://github.com/cloudfoundry-community/web-config-transform-buildpack 
				for performing token replaccement, transformation, etc.
============================================================================================================================================
============================================================================================================================================
Below are the important developer instructions, to follow after installation of this package
============================================================================================================================================

1. Please fing the json configuration details below. You can override these using custom configurations like environment variable or even json.

	{
	  "spring": {
		"application": {
		  "name": "${vcap.application.name}"
		},
		"cloud": {
		  "config": {
			"validate_certificates": false,
			"failFast": true,
			"name": "${vcap.application.name}"
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

============================================================================================================================================
