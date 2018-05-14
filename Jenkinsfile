node('master') {
    stage('import') {
        git 'https://github.com/1804-Apr-USFdotnet/Project2-Newsify.git'
    }
    stage('build') {
        try {
                bat 'nuget restore'
                bat 'msbuild'
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
            bat 'msbuild Newsify.Web /t:package'
        }
        catch (exc) {
            slackSend 'package failed!'
            throw exc
        }
    }
    stage('deploy') {
        try {
           // dir('./Newsify.Web/obj/Debug/Package')
           // {
            //        bat "\"C:\\Program Files\\IIS\\Microsoft Web Deploy V3\\msdeploy.exe\" -source:package='C:\\Program Files (x86)\\Jenkins\\workspace\\Project 1 Pipeline\\RRRaves.Web\\obj\\Debug\\Package\\RRRaves.Web.zip' -dest:auto,computerName=\"https://ec2-35-173-247-221.compute-1.amazonaws.com:8172/msdeploy.axd\",userName=\"Administrator\",password=\"${env.Deploy_Password}\",authtype=\"basic\",includeAcls=\"False\" -verb:sync -disableLink:AppPoolExtension -disableLink:ContentExtension -disableLink:CertificateExtension -setParamFile:\"C:\\Program Files (x86)\\Jenkins\\workspace\\Project 1 Pipeline\\RRRaves.Web\\obj\\Debug\\Package\\RRRaves.Web.SetParameters.xml\" -AllowUntrusted=True"
                
            //}
        }
        catch (exc) {
            slackSend 'deploy failed!'
            throw exc
        }
    }
}
