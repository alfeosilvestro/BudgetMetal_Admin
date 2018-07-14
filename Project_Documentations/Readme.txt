my.ini

innodb_log_file_size = 500M
max_allowed_packet = 100M

GRANT ALL PRIVILEGES ON *.* TO 'platform_user'@'%' IDENTIFIED BY 'password';

FLUSH PRIVILEGES;



<PublishWithAspNetCoreTargetManifest>false</PublishWithAspNetCoreTargetManifest>



The main problem for me was to reliably find out the IP address of the host. So in order to have a fixed set of IPs for both my host and my containers I’ve set up a docker network like this:

#	docker network create -d bridge --subnet 192.168.0.0/24 --gateway 192.168.0.1 dockernet

Now each container can connect to the host under the fixed IP 192.168.0.1.

You just need to make sure, that you connect all your containers to that “dockernet” network you just created. You can do that with the --net=dockernet option for docker run. Or from a docker-compose.yml:

version: '2'
services:
    db:
        image: some/image
        networks:
            - dockernet
networks:
    dockernet:
        external: true
Networks are described in the network documentation 3.7k. They are quite useful and not very hard to understand.

=================================================================================================================

Run in different port

The microsoft/aspnetcore-build container builds on top of the 
microsoft/aspnetcore container. The dockerhub page for that says:

#A note on ports

#This image sets the ASPNETCORE_URLS environment variable to http://+:80 which means that if you have not explicity set a URL in your #application, via app.UseUrl in your Program.cs for example, then your application will be listening on port 80 inside the container.

#So this is the container actively setting the port to 80. You can override it, if you want, by doing this in your Dockerfile:

**** ENV ASPNETCORE_URLS=http://+:5000

Also, it is worth noting that because of the docker command you are using, you will still be able to access the application at http://localhost:5000 whether you are running the application directly or in a container.
