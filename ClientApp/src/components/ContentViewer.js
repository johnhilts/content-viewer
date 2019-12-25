import React, { Component } from 'react';

const renderButton = (content, onClick) => {
    return (
        <button 
            type='button'
            className='link-button' 
            onClick={onClick}>
            {content}
        </button>
    )
}

const renderFolders = (content, contentName, onContentClick) => {
    return (
        <tr key={content.name}>
          <td>
            {renderButton(content.name, onContentClick.bind(null, contentName, content.contentType))}
          </td>
        </tr>
    )
}

const renderFiles = (content, contentName, onContentClick, currentFolder) => {
    const isImage = !contentName.endsWith('.mov')
    const image = <img src={`content/${currentFolder}/${contentName}`} alt={contentName} className='thumbnail' />
    const video = <video src={`content/${currentFolder}/${contentName}`} title={contentName} className='thumbnail' />
    const renderElement = isImage ? image : video

    return (
        <tr key={content.name}>
          <td>
            {renderButton(renderElement, onContentClick.bind(null, contentName, content.contentType))}
          </td>
        </tr>
    )
}

const renderContentTable = (contentInfo, onContentClick, currentFolder) => {
      const getContent = (contentInfo) => {
          const isFolder = contentInfo[0].contentType === 0
          const headerTitle = isFolder
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
                      return isFolder
                      ? renderFolders(content, contentName, onContentClick)
                      : renderFiles(contentInfo, contentName, onContentClick, currentFolder)
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
      : renderContentTable(folderData, props.onContentClick, props.currentFolder);

    let files = props.loading
      ? <p><em>Loading...</em></p>
      : renderContentTable(props.files, props.onContentClick, props.currentFolder);

    return (
      <div>
        <h1 id="tabelLabel" >Content Viewer</h1>
        <p>Select an item to view its content:</p>
        {folders}
        {files}
      </div>
    );
  }
