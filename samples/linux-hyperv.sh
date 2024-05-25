#!/bin/bash
# this is complete fantasy at the moment, more of a guide as to what i'm aiming for and will evolve quickly as I make stuff up

labName='TestLinux'

eval `fulab set global storage -l /var/tmp/gs` && export FULABLOCATION
fulab new service-defn -n HyperV-LAB --hypervisor HyperV --host LAB
fulab new lab-defn -n $labName -service HyperV-LAB
eval `fulab set current-lab $labName` && export FULABCURRENTLAB

fulab add virtualnetworkdefinition -n $labName --addressspace 192.168.8.0/24
fulab add virtualnetworkdefinition -n 'Default Switch' --hypervproperties "SwitchType = 'External'; AdapterName = 'Ethernet'"
fulab add machinedefinition --network $labName --toolspath $labSources\Tools --dnserver1 192.168.8.1 --gateway 192.168.8.1 --memory 1.5GB --operatingsystem CentOS-7

fulab add domaindefinition --name contoso.com 
fulab add installationcredential --username Install --password Somepass1

fulab build-lab
fulab start-lab
fulab stop-lab
fulab destroy-lab