Vagrant.configure("2") do |config|
  config.vm.box = "Ubuntu-Vagrant"
  config.vm.provider "virtualbox" do |vb|
    vb.memory = "4096"
    vb.cpus = 4
  end

  config.vm.provision "shell", inline: <<-SHELL
    apt-get update
    apt-get install -y docker.io
    systemctl start docker
    systemctl enable docker
    curl -L "https://github.com/docker/compose/releases/download/1.29.2/docker-compose-$(uname -s)-$(uname -m)" -o /usr/local/bin/docker-compose
    chmod +x /usr/local/bin/docker-compose
    cd /vagrant
    sudo docker-compose up
  SHELL
  config.vm.network "forwarded_port", guest: 5071, host: 5071
  config.vm.network "forwarded_port", guest: 5072, host: 5072
  config.vm.network "forwarded_port", guest: 3000, host: 3000
  config.vm.network "forwarded_port", guest: 9090, host: 9090
end