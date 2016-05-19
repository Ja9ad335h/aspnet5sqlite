FROM microsoft/aspnet:1.0.0-rc1-final

COPY . /app/
WORKDIR /app
RUN ["dnu", "restore"]

EXPOSE 5008
ENTRYPOINT ["dnx", "-p", "./src/WhatsappGroups/project.json", "web"]
