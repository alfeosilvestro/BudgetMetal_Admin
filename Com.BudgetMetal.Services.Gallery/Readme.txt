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