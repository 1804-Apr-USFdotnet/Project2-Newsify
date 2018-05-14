node('master') {
    stage('import') {
        git 'https://github.com/1804-Apr-USFdotnet/Project2-Newsify.git'
    }
    stage('build') {
        try {
            dir('./Newsify.Web/')
            {
                bat 'nuget restore'
                bat 'msbuild'
            }
        }
        catch (exc) {
            slackSend 'build failed!'
            throw exc
        }
    }
    stage('test') {
        try {
            
        }
        catch (exc) {
            slackSend color: 'danger', message: 'test failed!'
            throw exc
        }
    }
    stage('analyze') {
        try {

        }
        catch (exc) {
            slackSend 'analyze failed!'
            throw exc
        }
    }
    stage('package') {
        try {
            dir('./Newsify.Web/') {
            bat 'msbuild Newsify.Web /t:package'
            }
        }
        catch (exc) {
            slackSend 'package failed!'
            throw exc
        }
    }
    stage('deploy') {
        try {
           dir('./Newsify.Web/obj/Debug/Package')
           {
               bat "\"C:\\Program Files\\IIS\\Microsoft Web Deploy V3\\msdeploy.exe\" -source:package='C:\\Program Files (x86)\\Jenkins\\workspace\\Project2-Newsify\\Newsify.Web\\Newsify.Web\\obj\\Debug\\Package\\Newsify.Web.zip' -dest:auto,computerName=\"https://ec2-18-205-102-39.compute-1.amazonaws.com:8172/msdeploy.axd\",userName=\"Administrator\",password=\"${env.Deploy_PW}\",authtype=\"basic\",includeAcls=\"False\" -verb:sync -disableLink:AppPoolExtension -disableLink:ContentExtension -disableLink:CertificateExtension -setParamFile:\"C:\\Program Files (x86)\\Jenkins\\workspace\\Project2-Newsify\\Newsify.Web\\Newsify.Web\\obj\\Debug\\Package\\Newsify.Web.SetParameters.xml\" -AllowUntrusted=True"
                
            }
        }
        catch (exc) {
            slackSend 'deploy failed!'
            throw exc
        }
    }
}
