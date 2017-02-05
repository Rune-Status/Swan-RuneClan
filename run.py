import csv
import io
from urllib.request import urlopen

def Lookup():

    clan = input("Enter a clan name: ")
    api_link = 'http://services.runescape.com/m=clan-hiscores/members_lite.ws?clanName={0}'.format(clan)
    print ("Grabbing clan list...")

    try:
        api_contents = urlopen(api_link)
        data = csv.reader(io.TextIOWrapper(api_contents))

        # place clan member in its own list
        members = []
        for clan_member in data:
            members.append(clan_member[0])

        # Create text file for names w/ and w/o numbers
        txt = open("{0}.txt".format(clan), "w")
        txt_num = open("{0}_num.txt".format(clan), "w")

        for member in members:
            txt.write("{0}\n".format(member))
            # Write to another file with only names with numbers
            for i in range (0,9):
                if str(i) in member:
                    txt_num.write("{0}\n".format(member))
                    break
            continue

        print ("Created files: {0}.txt and {0}_num.txt".format(clan))
        txt.close()
        txt_num.close()
        input ("Press enter to exit...")

    except IndexError:
        print ("Error: Invalid Clan Name") 
        Lookup()

Lookup()        