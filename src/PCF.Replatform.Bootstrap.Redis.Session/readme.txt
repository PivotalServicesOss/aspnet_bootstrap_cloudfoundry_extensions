============================================================================================================================================
IMPORTANT NOTE: You can very well make use of the cf supply buildpack https://github.com/cloudfoundry-community/redis-session-aspnet-buildpack 
				for persisting session to redis. Supply buildpacks are preferred way to do.
============================================================================================================================================
============================================================================================================================================
Below are the important developer instructions, to follow after installation of this package
============================================================================================================================================

1. Please make sure to create the machine key from https://www.developerfusion.com/tools/generatemachinekey and replace the section in web.config accordingly

	Before (looks like):
	===================

	<machineKey validationKey="{Validation Key}"
                decryptionKey="{Decryption Key}"
                validation="SHA1" decryption="AES"/>

	After (looks like):
	===================

	<machineKey validationKey="838A7FA9D1747FA388E8F6100CE303116B3AFCA6C8A19955CFC75E2DB2D8938EFBD4575EB94C8F5C2B8874E80B5A49037571A4420BA2CE2A44A13738C45C32F7" 
				decryptionKey="D3A95572AC0CB2ED3DA4F2F7DAD21CD57684A609A102B8F5CAB47DECB1409FE1" 
				validation="SHA1" decryption="AES" />

2. The package will cleanup the sessionState section and create one as below.
	
	<sessionState mode="Custom" customProvider="RedisSessionStateStore">
      <providers>
        <add name="RedisSessionStateStore" 
			type="Microsoft.Web.Redis.RedisSessionStateProvider" 
			settingsClassName="PivotalServices.CloudFoundry.Replatform.Bootstrap.Base.Helpers.RedisConnectionHelper" 
			settingsMethodName="GetConnectionString" />
      </providers>
    </sessionState>

============================================================================================================================================