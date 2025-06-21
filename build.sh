mcs -target:library \
    -r:System.Web.dll \
    -r:bin/ZoomCharts.ProxyEcho.dll \
    -out:bin/GlobalHandlers.dll \
    Global.asax.cs
