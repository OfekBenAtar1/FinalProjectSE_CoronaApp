#!/bin/sh
# A little script that tests whether a .sln builds, and whether the 
# xUnit unit tests run. Made to work with git-hooks. Checkout the
# pre-commit.sample file for an example. This script makes use of a 
# .env file. If the .env file is not present, the .env.example file
# will be copied and used.

# Check if the .env file exists. If it does not, copy the .env.example
# file and name it .env.
if ! [ -e .env ]
then
  echo "Generating new .env file from .env.example."
  cp ./.env.example ./.env
fi

# Retrieve the contents of the .env file. The '^#' is a regex excluding
# all the lines with a # in front, in essence making them comments.
str="$(egrep -v '^#' .env)"

# Convert the string to an array. This array is split by newlines.
readarray -t vars <<<"$str"

# Loop through each read line, and declare the variable defined on that line.
for i in "${vars[@]}"
do
	declare "$i"
done

# Run the MSBuild command to check whether or not the code builds.
if eval $MSBUILD_PATH >/dev/null 2>&1
then
  echo Build OK
else
  exec 1>&2
  echo Build FAILED
  exit 1
fi

# Run xUnit to check whether or not the tests run succesfully.
if $XUNIT_CONSOLE_PATH $TESTS_DLL_PATH >/dev/null 2>&1
then
  echo Tests OK
else
  exec 1>&2
  echo Tests FAILED
  exit 1
fi
