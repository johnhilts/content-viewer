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
            {renderButton(content.name, onContentClick.bind(null, contentName, content.contentType, null, null))}
          </td>
        </tr>
    )
}
// my idea:
// button hits handler
// the handler (in the container) changes state + or - 90 (we'll put the degrees in state)
// instead of using a class defined in css, we'll add a STYLE whose content we dynamically set
const renderFiles = (content, contentName, onContentClick, currentFolder, currentFileIndex, showThumbnails, onRemoveClick, rotateDegrees) => {
    const photoContainer = document.getElementById('photoContainer')
    const maxHeight = photoContainer ? photoContainer.clientHeight : 800
    const maxWidth = photoContainer ? photoContainer.clientWidth : 1000
    const contentClass = showThumbnails ? 'thumbnail' : '' // 'responsiveImageLarge'
    const isImage = !contentName.endsWith('.mov')
    const image = <img src={`content/${currentFolder}/${contentName}`} alt={contentName} className={contentClass} style={{transform: `rotate(${rotateDegrees}deg)`, maxHeight: `${maxHeight}px`, maxWidth: 'auto'}} />
    const video = <video src={`content/${currentFolder}/${contentName}`} title={contentName} className={contentClass} controls />
    const renderElementType = isImage ? image : video
    const onClick = (fileIndex, rotateDegrees) => onContentClick.bind(null, contentName, content.contentType, fileIndex, rotateDegrees)
    const renderElement = renderButton(renderElementType, onClick(currentFileIndex))
    const renderPrevious = showThumbnails ? <span>&nbsp;</span> : renderButton(<span className='spacer'>Previous</span>, onClick(currentFileIndex-1))
    const renderNext = showThumbnails ? <div><div style={{marginLeft:'10px'}}>{content.name}</div><div>{content.created}</div><div>{content.geoCoordinateText}</div></div> : renderButton(<span className='spacer'>Next</span>, onClick(currentFileIndex+1))
    const removeStyle = showThumbnails ? 'spacer' : 'centeredSpacer'

    return (
        <tr key={content.name}>
          <td>
            <div id='photoContainer'>
                {renderPrevious}
                {renderElement}
                {renderNext}
            </div>
            <div className={removeStyle}>
                <span className={removeStyle}>{renderButton(<span>Rotate Left</span>, onClick(currentFileIndex, 90))}</span>
                <span className={removeStyle}><button onClick={onRemoveClick.bind(null, contentName)}>Remove</button></span>
                <span className={removeStyle}>{renderButton(<span>Rotate Right</span>, onClick(currentFileIndex, -90))}</span>
            </div>
          </td>
        </tr>
    )
}

const renderContentTable = (contentInfo, onContentClick, currentFolder, currentFileIndex, showThumbnails, onRemoveClick, rotateDegrees) => {
      const getContent = (contentInfo) => {
          const isFolder = contentInfo[0].contentType === 0
          const headerTitle = isFolder
          ? 'Folders'
          : 'Files'
          const viewContentInfo = currentFileIndex === -1 
          ? contentInfo
          : contentInfo.filter((f,i) => i === currentFileIndex)
        return (
            <table className='table table-striped' aria-labelledby="tabelLabel">
                <thead>
                  <tr>
                    <th>{headerTitle}</th>
                  </tr>
                </thead>
                <tbody>
                  {viewContentInfo.map(content => {
                          const contentName = content.name === 'Go Back ...'
                          ? '..'
                          : content.name
                      return isFolder
                      ? renderFolders(content, contentName, onContentClick)
                      : renderFiles(content, contentName, onContentClick, currentFolder, currentFileIndex, showThumbnails, onRemoveClick, rotateDegrees)
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
      : renderContentTable(folderData, props.onContentClick, props.currentFolder, props.currentFileIndex, props.showThumbnails);

    let files = props.loading
      ? <p><em>Loading...</em></p>
      : renderContentTable(props.files, props.onContentClick, props.currentFolder, props.currentFileIndex, props.showThumbnails, props.onRemoveClick, props.rotateDegrees);

    return (
      <div>
        <h1 id="tabelLabel" >Content Viewer</h1>
        <p>Select an item to view its content:</p>
        {folders}
        {files}
      </div>
    );
  }
