PROJ := mplanner
BIN := /usr/bin
FOLDER_PATH := /home/$(USER)/$(PROJ)

install: build strip make-folder copy

make-folder:
	mkdir -p $(FOLDER_PATH)

build:
	dotnet publish -c Release
	mv bin/Release/net7.0/linux-x64/publish/$(PROJ) $(PROJ)

strip:
	strip --strip-all $(PROJ)
	upx --brute $(PROJ)

copy:
	mv $(PROJ) $(BIN)
	chmod 755 $(BIN)/$(PROJ)
