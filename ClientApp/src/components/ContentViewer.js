import React, { Component } from 'react';

const renderContentTable = (contentInfo, onContentClick) => {
      const getContent = (contentInfo) => {
          const headerTitle = contentInfo[0].contentType === 0
          ? 'Folders'
          : 'Files'
        return (
            <table className='table table-striped' aria-labelledby="tabelLabel">
                <thead>
                  <tr>
                    <th>{headerTitle}</th>
                  </tr>
                </thead>
                <tbody>
                  {contentInfo.map(content => {
                          const contentName = content.name === 'Go Back ...'
                          ? '..'
                          : content.name
                      return (
                        <tr key={content.name}>
                          <td>
                              <button 
                                type="button"
                                className="link-button" 
                                onClick={onContentClick.bind(null, contentName, content.contentType)}>
                                  {content.name}
                              </button>
                          </td>
                        </tr>
                      )
                      }
                  )}
                </tbody>
            </table>
        )
      }

    const contents = contentInfo.length
      ?
      getContent(contentInfo)
      :
      <div>&nbsp;</div>

    return contents
  }

    const addReturnFolderData = (folderData) => {
        const folders = folderData ? folderData : []
        return [{name: 'Go Back ...', contentType: 0}, ...folders]
    }

export default function ContentViewer(props) {
    let folderData = addReturnFolderData(props.folders)
    let folders = props.loading
      ? <p><em>Loading...</em></p>
      : renderContentTable(folderData, props.onContentClick);

    let files = props.loading
      ? <p><em>Loading...</em></p>
      : renderContentTable(props.files, props.onContentClick);

    return (
      <div>
        <h1 id="tabelLabel" >Content Viewer</h1>
        <p>Select an item to view its content:</p>
        {folders}
        {files}
      </div>
    );
  }
