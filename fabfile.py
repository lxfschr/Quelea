from fabric.api import lcd,local
import os

def git():
    #local('git config --global credential.helper "cache --timeout=3600"')
    message = raw_input("Enter a git commit message:  ")
    local("git add . && git add -u && git commit -m \"%s\"" % message)
    local("git push")