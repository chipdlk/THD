/*
  RoxyFileman - web based file manager. Ready to use with CKEditor, TinyMCE. 
  Can be easily integrated with any other WYSIWYG editor or CMS.

  Copyright (C) 2013, RoxyFileman.com - Lyubomir Arsov. All rights reserved.
  For licensing, see LICENSE.txt or http://RoxyFileman.com/license

  This program is free software: you can redistribute it and/or modify
  it under the terms of the GNU General Public License as published by
  the Free Software Foundation, either version 3 of the License.

  This program is distributed in the hope that it will be useful,
  but WITHOUT ANY WARRANTY; without even the implied warranty of
  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
  GNU General Public License for more details.

  You should have received a copy of the GNU General Public License
  along with this program.  If not, see <http://www.gnu.org/licenses/>.

  Contact: Lyubomir Arsov, liubo (at) web-lobby.com
*/
function FileSelected(file){
  /**
   * file is an object containing following properties:
   * 
   * fullPath - path to the file - absolute from your site root
   * path - directory in which the file is located - absolute from your site root
   * size - size of the file in bytes
   * time - timestamo of last modification
   * name - file name
   * ext - file extension
   * width - if the file is image, this will be the width of the original image, 0 otherwise
   * height - if the file is image, this will be the height of the original image, 0 otherwise
   * 
   */
  //alert('"' + file.fullPath + "\" selected.\n To integrate with CKEditor or TinyMCE change INTEGRATION setting in conf.json. For more details see the Installation instructions at http://www.roxyfileman.com/install.");
   

    var integration = RoxyUtils.GetUrlParam('integration');
    if (integration == "custom") {
        
        $(window.parent.document).find('#customRoxyImage').attr('src', file.fullPath);
        $(window.parent.document).find('#Item_Images').attr('value', file.fullPath);
        window.parent.closeCustomRoxy();
    }
    else if (integration == "custom_2") {

        var fieldId = RoxyUtils.GetUrlParam('txtFieldId');
        var STR_FileName = '#FileName_' + fieldId.toString();
        var STR_lblFileName = '#lblFileName_' + fieldId.toString();
        $(window.parent.document).find(STR_lblFileName).attr('src', file.fullPath);
        $(window.parent.document).find(STR_FileName).attr('value', file.fullPath);
        window.parent.closeCustomRoxy2();
    }
    else {
        var fieldId = RoxyUtils.GetUrlParam('txtFieldId');
        console.log(fieldId);
        var STR_FileName = '#FileName_' + fieldId.toString();
        var STR_lblFileName= '#lblFileName_' + fieldId.toString();
        $(window.parent.document).find(STR_lblFileName).html(file.fullPath);
        $(window.parent.document).find(STR_FileName).attr('value', file.fullPath);        
        window.parent.closeCustomRoxy2();
    }
    

}
function GetSelectedValue(){
  /**
  * This function is called to retrieve selected value when custom integration is used.
  * Url parameter selected will override this value.
  */
  
  return "";
}
